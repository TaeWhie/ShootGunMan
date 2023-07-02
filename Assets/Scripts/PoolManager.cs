using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    public List<GameObject>[] pools;
    public int max;
    private int count = 0;
    private void Start()
    {

        pools = new List<GameObject>[prefabs.Length];

        for(int i=0; i<pools.Length;i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int i)
    {
        GameObject select = null;

        foreach(GameObject item in pools[i])
        {
            if (item != null&&!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                if (select.GetComponent<Enemy>())
                {
                    select.GetComponent<Enemy>().enabled = true;
                }
                return select;
            }
        }

        if(select==null&& Manager.Instance._pool.transform.childCount <max)
        {
            select = Instantiate(prefabs[i], transform);
            count++;
            select.name += count;
            pools[i].Add(select);
        }
        else
        {
            select = null;
        }


        return select;
    }
    public void SetMax(int m)
    {
        max = m;
    }

}
