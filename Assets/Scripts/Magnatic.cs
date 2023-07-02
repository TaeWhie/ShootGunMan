using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnatic : MonoBehaviour
{
    GameObject go;
    public float power;
    private void Update()
    {
       if( Manager.Instance.ReturnPlayer().GetComponent<PlayerInfo>().hp<=0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)//drops
        { 
            go = collision.gameObject;
            if (go.GetComponent<DropMove>() != null)
            {
                go.GetComponent<DropMove>().enabled = false;
                go.transform.position = Vector3.MoveTowards(go.transform.position, transform.position, Time.deltaTime * power);
            }
        }
    }
}
