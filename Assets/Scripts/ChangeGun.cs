using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class ChangeGun : MonoBehaviour
{
    public Sprite[] guns;
    private int saveActiveWeapon = -1;
    // Start is called before the first frame update
    void Start()
    {
        Manager.Instance._data.allComplete.Where(x => x == true).Subscribe(dats =>
        {
            guns = Manager.Instance._data.allSprites[1];
            Manager.Instance.ReturnPlayer().GetComponent<PlayerInfo>().activeWeapon.Where(x => x != saveActiveWeapon).
            Subscribe(data => { ChangeSprite(); });
        });
    }

    public void ChangeSprite()
    {
        if (Manager.Instance.ReturnPlayer().GetComponent<PlayerInfo>().activeWeapon.Value != -1)
        {
            saveActiveWeapon = Manager.Instance.ReturnPlayer().GetComponent<PlayerInfo>().activeWeapon.Value;
            gameObject.GetComponent<SpriteRenderer>().sprite = guns[saveActiveWeapon];
        }
    }
}
