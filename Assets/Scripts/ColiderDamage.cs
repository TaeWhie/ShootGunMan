using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderDamage : MonoBehaviour
{
    [SerializeField] private bool trigger = false;
    [SerializeField] public float damage;
    [SerializeField] public bool destroyByTime = false;
    public float totalDamage;
    private Enemy enemy;

    private void Start()
    {
        ResetDamage();
    }
    private void OnEnable()
    {
        if (transform.GetChild(0).GetComponent<CapsuleCollider2D>()&&destroyByTime)
        {
            transform.GetChild(0).GetComponent<CapsuleCollider2D>().enabled = true;
        }
        ResetDamage();
        totalDamage = damage+Manager.Instance.ReturnPlayer().GetComponent<PlayerInfo>().
            PlayerLevels["DamageLevel"]*25;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemy = collision.gameObject.GetComponent<Enemy>())//태그?레이어?
        {
            enemy.hp -= totalDamage;

            if (!trigger&&!destroyByTime)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        if(transform.GetChild(0).GetComponent<CapsuleCollider2D>())
        transform.GetChild(0).GetComponent<CapsuleCollider2D>().enabled = false;
    }
    void ResetDamage()
    {
        damage = float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.BulletChart].
            stringchart[gameObject.GetComponent<ShootStraight>().bulletID + 1, 4]);
    }
}
