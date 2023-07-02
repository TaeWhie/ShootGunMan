using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class EXP : MonoBehaviour
{
    public Color[] level = new Color[3];
    public List<GameObject> exps = new List<GameObject>();
    public int value = 1;
    public float radius = 0.5f;
    public int total=0;
    public GameObject deleter;
    // Update is called once per frame
    private ReactiveProperty<Collider2D[]> colliders = new ReactiveProperty<Collider2D[]>();

    private void Start()
    {
        deleter = Manager.Instance.expDeleter;
    }
    private void OnEnable()
    {
        gameObject.GetComponent<SpriteRenderer>().color = level[0];
        total = value;

        colliders.Where(x=>x!=null).Subscribe(data =>
        {
            total = value;
            for (int i = 0; i < colliders.Value.Length; i++)
            {
                total += colliders.Value[i].GetComponent<EXP>().value;
            }

            if (total >= 5)
            {
                deleter.transform.position = this.transform.position;
                Debug.Log(total);
                deleter.GetComponent<ExpDeleter>().total = total;
                deleter.SetActive(true);
            }
        });
    }
    void Update()
    {
        colliders.Value = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Drops"));
        // WrapAroundCameraView(gameObject.GetComponent<SpriteRenderer>());
    }
    private void WrapAroundCameraView(SpriteRenderer render)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > -0.1 && screenPoint.x < 1.1 && screenPoint.y > -0.1 && screenPoint.y < 1.1;

        if (!onScreen)
        {
            render.enabled = false;
        }
        else
        {
            render.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
