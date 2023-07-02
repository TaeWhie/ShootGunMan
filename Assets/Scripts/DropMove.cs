using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMove : MonoBehaviour
{
    public float speed;

    public float maxDistance;

    private float ranDistance;

    public  Vector3 startPos;

    // Start is called before the first frame update
    private Vector3 randomPos;



    private void OnEnable()
    {
        startPos = transform.position;
        ranDistance = Random.Range(-maxDistance, maxDistance);
        randomPos.x = startPos.x+ranDistance;
        ranDistance = Random.Range(-maxDistance, maxDistance);
        randomPos.y = startPos.y+ranDistance;
    }
    private void Update()
    {
        transform.position=Vector3.Lerp(transform.position, randomPos, Time.deltaTime*speed);
        if(transform.position==randomPos)
        {
            gameObject.GetComponent<EXP>().enabled = true;
            enabled = false;
        }
    }
    private void OnDisable()
    {
        enabled = false;
    }
  
}
