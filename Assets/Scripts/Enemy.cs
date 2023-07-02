using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class Enemy : MonoBehaviour
{
    protected WaveManager emng;
    protected Manager mng;
    public Color targetColor;
    private float timer;
    private float maxtime = 2.0f;

    public ReactiveProperty<int> id = new ReactiveProperty<int>();

    private int nowId = 0;
    public string name;
    public float damage;
    public int level;
    public float hp;
    public float speed = 0.5f;
    public float totalSpeed;
    public int expCount = 10;

    private string[] sentence;
    private AI ai = new();
    public bool setting = false;

    public float saveHP;
    private float MaxHP;
    private bool onCoroutine = false;

    GameObject drop;

    IEnumerator BlinkSprite()
    {
        onCoroutine = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(0.03f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.15f);
        onCoroutine = false;
        StopCoroutine("BlinkSprite");
    }

    private void OnEnable()
    {
        setting = id.Value == nowId ? true : false;
        hp = MaxHP;
        onCoroutine = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        id.Where(x => x != nowId && mng._data.allComplete.Value == true).Subscribe(data => { SetProfile(); }).AddTo(gameObject);
    }
    private void Awake()
    {
        mng = Manager.Instance;

    }

    private void Update()
    {
        if (setting == true)
        {
            WrapAroundCameraView(gameObject.GetComponent<SpriteRenderer>());
            ai.onUpdate();
            FixSpeed();
            if (saveHP != hp)
            {
                saveHP = hp;
                if(onCoroutine==false&&gameObject.activeSelf)
                StartCoroutine("BlinkSprite");
            }
            Die();
        }
    }
    
    public void SetProfile()
    {
        sentence = Manager.Instance._data.ReadRow(id.Value, sentence, (int)DataManager.ChartName.MonsterChart);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        switch (id.Value)
        {
            case 1:
                ai = new GreenOak(gameObject);
                break;
            case 2:
                ai = new GreenOak(gameObject);
                break;
        }

        for (int j = 1; j < Manager.Instance._data.chartInfos[(int)DataManager.ChartName.MonsterChart].rowSize; j++)
        {
            switch (j)
            {
                case 1:
                    name = sentence[j];
                    break;
                case 2:
                    damage = float.Parse(sentence[j]);
                    break;
                case 3:
                    level = int.Parse(sentence[j]);
                    break;
                case 4:
                    MaxHP = float.Parse(sentence[j]);
                    hp = MaxHP;
                    saveHP = hp;
                    break;
            }
        }
        nowId = id.Value;
        setting = true;
    }
    public void Die()
    {
        if (hp <= 0)
        {
            int ran = Random.Range(0, 100);
            GameObject drop;
            Manager.Instance._wave.spawn.spawncount[id.Value - 1] -= 1;
            if (ran > 5)
            {
                for (int i = 0; i < expCount; i++)
                {
                    drop = Manager.Instance._drop.Get(0);
                    drop.GetComponent<EXP>().value = 1;
                    drop.transform.position = transform.position;
                    drop.GetComponent<DropMove>().enabled = true;
                }
            }
            else
            {
                drop = Manager.Instance._drop.Get(1);
                drop.transform.position = transform.position;
            }
            gameObject.GetComponent<Enemy>().enabled = false;
            gameObject.SetActive(false);
        }
    }
    private void WrapAroundCameraView(SpriteRenderer render)
    {

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > -0.1 && screenPoint.x < 1.1 && screenPoint.y > -0.1 && screenPoint.y < 1.1;

        if (!onScreen)
        {
            render.enabled = false;
        }
        else
        {
            render.enabled = true;
        }
    }
    private void FixSpeed()
    {
        float dis= Vector3.Distance(mng.ReturnPlayer().transform.position, transform.position);
        totalSpeed = speed * 2 / dis;
        if (totalSpeed >= speed)
        {
            totalSpeed = speed;
        }
        else if(totalSpeed < 0.1)
        {
            gameObject.SetActive(false);
            Manager.Instance.Spawner.GetComponent<Spawner>().spawncount[id.Value-1] =
                Manager.Instance.Spawner.GetComponent<Spawner>().spawncount[id.Value - 1] -1;
        }

    }
}
