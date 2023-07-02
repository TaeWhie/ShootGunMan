using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIdleMove : MonoBehaviour
{
    [SerializeField] private float distance = 0.1f;
    // Start is called before the first frame update
    Vector3 startpos;
    private void Start()
    {
        startpos = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, startpos) > Mathf.Abs(distance) - 0.05f)
        {
            distance *= -1;
        }

        transform.transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * distance, Time.deltaTime*0.5f);
    }
}
