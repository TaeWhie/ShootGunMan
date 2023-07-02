using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDeleter : MonoBehaviour
{
    public EXP exp;
    public int total;
    public bool use=true;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (use)
        {
            StartCoroutine("UseDelete");
        }
    }
    IEnumerator UseDelete()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, exp.radius, 1 << LayerMask.NameToLayer("Drops"));
        if (total >= 5)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].gameObject.SetActive(false);
            }

            GameObject obj = Manager.Instance._drop.Get(0);
            obj.transform.position = transform.position;
            obj.GetComponent<DropMove>().enabled = true;

            obj.GetComponent<EXP>().value = total;

            if (total > 5 && total <= 10)
            {
                obj.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (total > 10)
            {
                obj.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        } 
        use = false;
        yield return new WaitForSeconds(10f);
        use = true;
        gameObject.SetActive(false);
    }
}
