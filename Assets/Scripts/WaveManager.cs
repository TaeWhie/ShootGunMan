using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class WaveManager: MonoBehaviour
{
   
    public int max;
    public Spawner spawn;
    public List<int> enemycount=new();

    public int PlayerLevel=0;
    private string[] sentence;

    private bool setting = false;
    public void init()
    {
        SetWave();
        setting = true;
    }
    public void OnUpdate()
    {
        if (setting == true)
        {
            spawn.onUpdate();
            if (Manager.Instance.PlayerLevel != PlayerLevel)
            {
                ResetEnemy();

                if (PlayerLevel != 0)
                {
                    SetWave();
                }
                PlayerLevel = Manager.Instance.PlayerLevel;
            }
        }
    }
    public void SetWave()
    {
       sentence = Manager.Instance._data.ReadRow(Manager.Instance.PlayerLevel, sentence, (int)DataManager.ChartName.WaveChart);
       max = int.Parse(sentence[1]);
       Manager.Instance._pool.SetMax(max);
       enemycount = new();

        for (int j = 2; j < Manager.Instance._data.chartInfos[(int)DataManager.ChartName.WaveChart].rowSize; j++)
       {
           enemycount.Add(int.Parse(sentence[j]));
       }
    }
    public bool levelup()
    {
        return Manager.Instance.PlayerLevel != PlayerLevel;
    }
    public void ResetEnemy()
    {
        Transform[] childList = Manager.Instance._pool.GetComponentsInChildren<Transform>();

        if(childList!=null)
        {
            for(int i=1; i<childList.Length;i++)
            {
                if(childList[i]!=transform)
                {
                    childList[i].gameObject.GetComponent<Enemy>().enabled = false;
                    childList[i].gameObject.SetActive(false);
                }
            }
        }
    }
}

