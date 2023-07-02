using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    public GameObject fire;
    private Animator fireani;
    // Start is called before the first frame update
    public float timer = 0;

    public float saveHp;

    private void Start()
    {
        fireani = fire.GetComponent<Animator>();
        saveHp = gameObject.GetComponent<PlayerInfo>().hp;

    }
    public void SetFire(bool a)
    {
        fireani.SetBool("Shoot", a);
    }
    private void Update()
    {
        fire.transform.position= transform.GetChild(0).GetChild(0).GetChild(0).transform.position;
        timer += Time.deltaTime;
        if(timer>=0.2f)
        {
            SetFire(false);
        }
        if (saveHp != gameObject.GetComponent<PlayerInfo>().hp )
        {
            saveHp = gameObject.GetComponent<PlayerInfo>().hp;
            StartCoroutine("EndInvincible",1);
        }
    }
    IEnumerator EndInvincible(int a)
    {
        float normalspeed = gameObject.GetComponent<PlayerController>()._speed;
        gameObject.GetComponent<PlayerController>()._speed = normalspeed*1.5f;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        for(int i=0;i<=3;i++)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.2f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.GetComponent<PlayerController>()._speed = normalspeed;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        StopCoroutine("EndInvincible");
    }
}
