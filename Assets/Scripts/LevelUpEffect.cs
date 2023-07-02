using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEffect : MonoBehaviour
{
    public float speed = 10;
    private Vector3 fullsize;
    float height;
    float width;
    private float full;
    // Start is called before the first frame update
    
    void OnEnable()
    {
         height = 2 * Camera.main.orthographicSize;
         width = height * Camera.main.aspect;
        if(height>=width)
        {
            full = height;
        }
        else
        {
            full = width;
        }
        fullsize = new Vector3(full * 2, full * 2, 0);

        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Manager.Instance.ReturnPlayer().transform.position;
        transform.localScale = Vector3.Lerp(transform.localScale, fullsize, Time.deltaTime * speed);
        if(transform.localScale.x >= fullsize.x-1)
        {
            Destroy(gameObject);
        }
    }
}
