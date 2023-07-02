using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPspawner : MonoBehaviour
{
    public int fullCount;
    [SerializeField] Transform EXP;
    private int count=0;
    private Transform child;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
 
    }
    IEnumerator Spawn()
    {
        while (count < fullCount)
        {
            child = Instantiate(EXP, transform.position, transform.rotation);
            child.transform.parent = this.transform;
            child.GetComponent<DropMove>().maxDistance = fullCount / 10 * 0.01f;
            count++;
            yield return new WaitForSeconds(1/fullCount);
        }
        yield break;
    }
}
