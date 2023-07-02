using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCase : MonoBehaviour
{
    int numofChild;
    public int bulletForm = 0;//총알인지, 미사일 인지, 불인지
    public int upgradeForm = 0;//업그레이드 방식이 총에 따라 달라지는거

    // Start is called before the first frame update
    private int activeWeapo;
    private int upgradeLevel;
    private Transform middlePoint;
    private PlayerInfo playerinfo;

    public int shootTry = 0;
    private void OnEnable()
    {
        playerinfo = Manager.Instance.ReturnPlayer().GetComponent<PlayerInfo>();
        middlePoint = gameObject.transform.GetChild(0).GetChild(0);
        activeWeapo = playerinfo.activeWeapon.Value;
        upgradeLevel = playerinfo.weapoNum[playerinfo.activeNum].upgradeLevel;
        bulletForm = int.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart].stringchart[activeWeapo+1, 3]);
        upgradeForm= int.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart].stringchart[activeWeapo+1, 4]);
        ManageBullet();
    }

    public void ManageBullet()
    {
        shootTry++;
        GameObject newBullet;
        switch (upgradeForm)
        {
            case 0://Rifle
                {
                    if (upgradeLevel > 0 && shootTry == 6 - upgradeLevel)
                    {
                        newBullet = Manager.Instance._bullet.Get(Manager.Instance._bullet.prefabs.Length-1);
                        newBullet.GetComponent<ShootStraight>().bulletID = int.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart].
                                  stringchart[activeWeapo + 1, 3]);
                        newBullet.GetComponent<ColiderDamage>().totalDamage *=float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.BulletChart].
                    stringchart[newBullet.GetComponent<ShootStraight>().bulletID + 1, 6]); ;
                        shootTry = 0;
                    }
                    else
                    {
                        newBullet = Manager.Instance._bullet.Get(bulletForm);
                    }
                    newBullet.transform.position = middlePoint.position;
                    newBullet.transform.rotation = middlePoint.rotation;
                    newBullet.GetComponent<ShootStraight>().setSlowly(false, 0);
                }
            break;
            case 1://shotgun
                {
                    Vector3 modifyVector = Vector3.zero;


                    for (int i = -Mathf.FloorToInt((upgradeLevel + 2) / 2f);
                         i <= Mathf.FloorToInt((upgradeLevel + 2) / 2f); i++)
                    {
                        modifyVector = Vector3.zero;
                        newBullet = Manager.Instance._bullet.Get(bulletForm);
                        newBullet.GetComponent<ShootStraight>().bulletID = int.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart].
                                    stringchart[activeWeapo + 1, 3]);
                        newBullet.transform.position = middlePoint.position;
                        newBullet.GetComponent<ShootStraight>().setSlowly(true, 15);
                        if (i != 0)
                        {
                            modifyVector = middlePoint.right * (0.25f / (upgradeLevel + 1)) * i;
                            newBullet.transform.position += modifyVector;
                        }

                        newBullet.transform.up = (middlePoint.up + modifyVector);
                    }
                }
                break;
            case 2://Missle
                {
                    newBullet = Manager.Instance._bullet.Get(bulletForm);
                    newBullet.transform.position = middlePoint.position;
                    newBullet.transform.rotation = middlePoint.rotation;

                    if (upgradeLevel != 0)
                    {
                        newBullet.transform.localScale +=Vector3.one*float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.BulletChart].
                      stringchart[newBullet.GetComponent<ShootStraight>().bulletID + 1, 5]) * upgradeLevel;
                    }
                    newBullet.GetComponent<ShootStraight>().bulletID = int.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart].
                              stringchart[activeWeapo + 1, 3]);
                    newBullet.GetComponent<ColiderDamage>().totalDamage += float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.BulletChart].
                  stringchart[newBullet.GetComponent<ShootStraight>().bulletID + 1, 6])*upgradeLevel;
                }
                break;
            case 3://FireMaker
                {
                    newBullet = Manager.Instance._bullet.Get(bulletForm);
                    newBullet.transform.position = middlePoint.position;
                    newBullet.transform.rotation = middlePoint.rotation;

                    newBullet.GetComponent<DieTimer>().Maxtimer += float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.BulletChart].
                  stringchart[newBullet.GetComponent<ShootStraight>().bulletID + 1, 5])* upgradeLevel;
                    newBullet.GetComponent<ColiderDamage>().totalDamage += float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.BulletChart].
                        stringchart[newBullet.GetComponent<ShootStraight>().bulletID + 1, 6])* upgradeLevel;

                    newBullet.GetComponent<ShootStraight>().bulletID=int.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart].
                               stringchart[activeWeapo + 1, 3]);
                }
                break;
        }
        this.enabled = false;
    }

}
