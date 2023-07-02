using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
    public bool useRandom;
    public int GunID;

    private void OnEnable()
    {
        if (useRandom)
        {
            int ran = Random.Range(0, Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart].lineSize - 1);
            GunID = ran;
        }
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
            Manager.Instance._data.allSprites[(int)DataManager.ChartName.GunChart][GunID];
    }
}
