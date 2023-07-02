using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float delaytime;
    public int[] spawncount ;
    private float radius; 

    private Manager mng;
    float timer;

    private void Awake()
    {
        mng = Manager.Instance;
        float height = 2 * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        radius = height > width ? height : width;
        radius /= 1.5f;
    }

  
    // Update is called once per frame
    public void onUpdate()
    {
        transform.position=mng.ReturnPlayer().transform.position;
        ResetArray();
        TimeSpawn();
    }
    void Spawn(int i)
    {
        GameObject enemy=Manager.Instance._pool.Get(0);
        if (enemy != null)
        {
            enemy.GetComponent<SpriteRenderer>().sprite = Manager.Instance._data.allSprites[0][i];
            enemy.GetComponent<Enemy>().id.Value = i+1;

            float deg=Random.Range(0, 360);
            float rad = Mathf.Deg2Rad * (deg);
            float x = radius * Mathf.Sin(rad);
            float y = radius * Mathf.Cos(rad);
     
            enemy.transform.position = transform.position + new Vector3(x, y);
        }
    }
    void ResetArray()
    {
        if (Manager.Instance._wave.levelup())
        {
            spawncount = new int[mng._wave.enemycount.Count];
            for (int i = 0; i < mng._wave.enemycount.Count; i++)
            {
                spawncount[i] = 0;
            }
        }
    }
    void TimeSpawn()
    {
        timer += Time.deltaTime;
        delaytime = 60 / Manager.Instance._pool.max;
        int ran = Random.Range(0, mng._wave.enemycount.Count);
        if (timer >= delaytime)
        {
            if (spawncount[ran] < mng._wave.enemycount[ran])
            {
                Spawn(ran);
                spawncount[ran] = spawncount[ran] + 1;
            }
            else
            {
                for (int i = 0; i < mng._wave.enemycount.Count; i++)
                {
                    if (ran == mng._wave.enemycount.Count - 1)
                    {
                        ran = 0;
                    }

                    if (spawncount[ran] != mng._wave.enemycount[ran])
                    {
                        Spawn(ran);
                        break;
                    }
                    else
                    {
                        ran++;
                    }

                }
            }
            timer = 0;
        }
    }
}
