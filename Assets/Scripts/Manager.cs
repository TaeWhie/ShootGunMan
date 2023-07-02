using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class Manager : MonoBehaviour
{
    private GameObject Player;
    public GameObject Spawner;
    public GameObject expDeleter;
    static Manager s_Instance;
    public WaveManager _wave;
    public ParticleManager _particle;
    InputManager _input = new();
    public UIManager _UI;
    public PoolManager _pool;
    public PoolManager _bullet;
    public PoolManager _drop;
    public DataManager _data;
    public int PlayerLevel = 1;  
    public bool pc;
    [SerializeField] private Texture2D[] _cursorimg;


    public int[] stageUp;

    public bool playerAlive = true;

    public static InputManager Input 
    {
        get { return Instance._input; } 
    }
    public static Manager Instance
    {
        get { Init(); return s_Instance; }
    }
    public  GameObject ReturnPlayer()
    {
        return Player;
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Player = GameObject.Find("Player");
        Time.timeScale = 1f;
    }
    private void Start()
    {
        Init();
        _data.Init();
        _data.allComplete.Where(x => x == true).Subscribe(data => {
            _UI.init();
            _wave.init();
        }).AddTo(gameObject);
    }
    private void Update()
    {
        onUpdate();
        _data.allComplete.Where(x => x == true).Subscribe(data => {
            if (playerAlive)
            {
                _input.onUpdate();
            }
            _particle.OnUpdate();
            _wave.OnUpdate();
            _UI.onUpdate();
        }).AddTo(gameObject);
    } 
    public void onUpdate()
    {
        if (stageUp.Length > PlayerLevel)
        {
            if (Player.GetComponent<PlayerInfo>().exp >= stageUp[PlayerLevel - 1])
            {
                PlayerLevel += 1;
            }
        }
        playerAlive = Player.GetComponent<PlayerInfo>().hp > 0 ? true : false;
    }

    static void Init()
    {
        if (s_Instance == null)
        {

            GameObject go = GameObject.Find("@Manager");

            if (go == null)
            {
 
                go = new GameObject { name = "@Manager" };
                go.AddComponent<Manager>();

            }

            s_Instance = go.GetComponent<Manager>();
            s_Instance._data.allComplete.Where(x => x == true).Subscribe(data => s_Instance._UI.Loading.SetActive(false));
        }
    }
    public void ReStart()
    {      
        Destroy(this);
        SceneManager.LoadScene("Title");
    }
    public void StopScene()
    {
        Time.timeScale = 0;
    }
    public void StartScene()
    {
        Time.timeScale = 1;
    }
}
