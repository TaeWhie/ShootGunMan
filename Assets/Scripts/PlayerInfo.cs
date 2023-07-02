using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerInfo : MonoBehaviour
{
    public float hp;
    public float mp;
    public float exp;

    public Dictionary<string, int> PlayerLevels = new Dictionary<string, int>()
    {
        { "HPLevel",0},
        { "MPLevel",0},
        { "DamageLevel",0},
        { "ShootRateLevel",5},
        { "MagnaticLevel",0},
        { "SpeedLevel",0},
    };
    public equipinfo[] weapoNum = new equipinfo[3];
    // public int activeWeapon = 0;
    public ReactiveProperty<int> activeWeapon = new ReactiveProperty<int>();
    public int activeNum = 0;
    public int level=1;
    public float shootcooltime = 0.2f;
    public struct equipinfo
    {
        public int Num;
        public int upgradeLevel;

        public void SetLevel(int level)
        {
            this.upgradeLevel = level;
        }
    }
    private void Start()
    {
        weapoNum[0].Num = 0;
        weapoNum[1].Num = 1;
        weapoNum[1].upgradeLevel = 0;
        weapoNum[2].Num = -1;
    }
    public void Update()
    {
        if (level != Manager.Instance.PlayerLevel)
        {
            exp = 0;
            level = Manager.Instance.PlayerLevel;
        }
        if(hp<=0)
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }

        SetZero(hp);
        SetZero(mp);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer== LayerMask.NameToLayer("Weapon"))
        {
            if(collision.gameObject.tag=="Gun")
            {
                int GunID = collision.gameObject.GetComponent<GunInfo>().GunID;
              
                for (int i=0 ; i<3 ;i++)
                {
                   if(weapoNum[i].Num== GunID)
                   {
                        Debug.Log("Weapon Upgrade");
                        weapoNum[i].upgradeLevel++;
                        weapoNum[i].upgradeLevel=Mathf.Clamp(weapoNum[i].upgradeLevel, 0, 5);

                        if (weapoNum[i].Num == 0)
                        {
                            shootcooltime -= float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart].
                                stringchart[weapoNum[i].Num + 1, 5]);
                        }
                        gameObject.GetComponent<BulletCase>().shootTry = 0;
                        break;
                   }
                   else if(weapoNum[i].Num == -1)
                   {
                        weapoNum[i].Num = collision.gameObject.GetComponent<GunInfo>().GunID;
                        Manager.Instance._UI.ChangeBoxImage();
                        break;
                   }
                   else
                   {
                        if (i == 2)
                        {
                            Debug.Log("select Weapon");
                            break;
                        }
                   }
                }  
            }
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Drops"))
        {
                exp += collision.GetComponent<EXP>().value;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            hp -= collision.gameObject.GetComponent<Enemy>().damage;
        }
    }
    public void SetZero(float a)
    {
        if(a<0)
        {
            a = 0;
        }
    }
    public void SetAciveWeapon(int num)
    {
        if (weapoNum[num].Num != -1)
        {
            ChartInfo Gunchart = Manager.Instance._data.chartInfos[(int)DataManager.ChartName.GunChart];
            activeNum = num;
            activeWeapon.Value = weapoNum[num].Num;
            shootcooltime = float.Parse(Gunchart.stringchart[weapoNum[num].Num + 1, 2]) - PlayerLevels["ShootRateLevel"] * 0.03f;
            shootcooltime -= weapoNum[num].upgradeLevel * float.Parse(Gunchart.stringchart[weapoNum[num].Num + 1, 5]);
            gameObject.GetComponent<BulletCase>().shootTry = 0;
        }
    }
}
