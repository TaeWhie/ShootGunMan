using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootStraight : MonoBehaviour
{
    public int bulletID;
    [SerializeField] private float speed = 8;
    [SerializeField] private bool bigger = false;
    [SerializeField] private bool spawnRandRote = false;
    [SerializeField] private bool shootRotate = false;
    [SerializeField] private float rotateSpeed = 0;
    [SerializeField] private float bigSpeed = 0;
    [SerializeField] private bool slowly = false;
    [SerializeField] private float acc = 0;
    [SerializeField] private float slowPercent = 0;
    [SerializeField] private bool returnBullet = false;
    private Vector3 normalSize;

    private void Start()
    {
        ResetSpeed();
    }
    private void FixedUpdate()
    {
        if (slowly)
        {
            acc += 0.01f*slowPercent;
            if(!returnBullet&&acc>speed)
            {
                gameObject.SetActive(false);
            }
        }

        transform.position += transform.up * (Time.deltaTime*speed- acc * Time.deltaTime);
     

        if(bigger)
        {
            transform.localScale += Vector3.one*bigSpeed * Time.deltaTime;
        }
        if(shootRotate)
        {
            gameObject.transform.GetChild(0).Rotate(Vector3.forward * rotateSpeed);
        }
    }
    private void OnEnable()
    {
        ResetSpeed();
        normalSize = transform.localScale;
        acc = 0;
    }
    private void OnDisable()
    {
        transform.localScale=normalSize;

        if (spawnRandRote)
        {
            gameObject.transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        }
    }

    public void setSlowly(bool set,float percent)
    {
        slowly = set;
        slowPercent = percent;
    }
    void ResetSpeed()
    {
        speed = float.Parse(Manager.Instance._data.chartInfos[(int)DataManager.ChartName.BulletChart].
           stringchart[gameObject.GetComponent<ShootStraight>().bulletID + 1, 2]);
    }

}
