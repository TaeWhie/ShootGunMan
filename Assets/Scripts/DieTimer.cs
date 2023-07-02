using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTimer : MonoBehaviour
{
    public float Maxtimer = 10.0f;
    public float timer;
    Transform[] childList;
    private void Start()
    {
        ResetMaxTimer();
    }
    private void OnEnable()
    {
        ResetMaxTimer();
        timer = 0;
    }
    void Update()
    { 
        timer += Time.deltaTime;

        if (timer>Maxtimer)
        {
            Die();
        }
    }
    public void Die()
    {
        timer = 0;
        gameObject.SetActive(false);
    }
    public void ResetMaxTimer()
    {
        if (gameObject.GetComponent<ShootStraight>())
        {
            Maxtimer = float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.BulletChart].
                stringchart[gameObject.GetComponent<ShootStraight>().bulletID + 1, 3]);
        }
    }
}
