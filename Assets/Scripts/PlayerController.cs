using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerController : MonoBehaviour
{
    public float _speed = 1;

    public Vector2 inputVec;
    [SerializeField]private Transform LookPoint;

    private PlayerInfo info;

    private bool cooltime = false;
    // Start is called before the first frame update
    void Start()
    {
        info = gameObject.GetComponent<PlayerInfo>();

#if UNITY_EDITOR
        Manager.Input.keyaction -= OnKeyboard;
        Manager.Input.keyaction += OnKeyboard;
#endif

        Manager.Instance._data.allComplete.Where(x => x == true).Subscribe(data => {gameObject.GetComponent<PlayerInfo>().SetAciveWeapon(0);
            StartCoolTime();});
    }
    private void Update()
    {
        if (Manager.Instance.playerAlive)
        {
            MathMover.Lookat(LookPoint.position, transform);
        }

    }
    void OnKeyboard()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        Vector3 Position = transform.position;

        Position.x += Horizontal * Time.deltaTime * _speed;
        Position.y += Vertical * Time.deltaTime * _speed;

        inputVec=new Vector2(Horizontal, Vertical);

        transform.position = Position;

        WeaponSwitch(KeyCode.Alpha1);
        WeaponSwitch(KeyCode.Alpha2);
        WeaponSwitch(KeyCode.Alpha3);
    }

    public void WeaponSwitch(KeyCode a)
    {
        if (Input.GetKeyDown(a))
        {
            gameObject.GetComponent<PlayerInfo>().SetAciveWeapon((int)a-49);
            Manager.Instance._UI.ChangeGunBox(Manager.Instance._UI.GunBox[(int)a-49]);
        }
    }
    public void Move(Vector2 inputDirection)
    {
        if (Manager.Instance.playerAlive)
        {
            Vector2 moveInput = inputDirection;
            Vector3 Position = transform.position;

            Position.x += inputDirection.x * Time.deltaTime * _speed;
            Position.y += inputDirection.y * Time.deltaTime * _speed;

            transform.position = Position;
        }
    }
    public void RotateShoot(Vector2 Direction)
    {
        if (Manager.Instance.playerAlive)
        {
            transform.up = Direction;
        }
    }
    public void StartCoolTime()
    {
        if (Manager.Instance.playerAlive)
        {
            StartCoroutine("ShootCoolTime");
        }
    }
    public void EndCoolTime()
    {
        StopCoroutine("ShootCoolTime");
    }
    IEnumerator ShootCoolTime()
    {
        OneShootSet();
        cooltime = true;
        yield return new WaitForSeconds(gameObject.GetComponent<PlayerInfo>().shootcooltime);
        cooltime = false;
        StartCoolTime();
    }
    private void OneShootSet()
    {
        gameObject.GetComponent<BulletCase>().enabled = true;
        gameObject.GetComponent<PlayerAni>().SetFire(true);
        gameObject.GetComponent<PlayerAni>().timer = 0;
    }
}
