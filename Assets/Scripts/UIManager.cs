using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
public class UIManager:MonoBehaviour
{
    //ivate int Cursornum = 0;
    
    public float UISize = 1;

    public GameObject Loading;
    public Canvas canvas;
    public Transform Hp;
    public Transform Mp;
    public Transform EXP;
    public Transform[] GameOver;
    public Transform[] GunBox;
    public Transform SelectedGun;

    private float saveHp = 100;
    private float saveMp = 100;

    private PlayerInfo player;
    public int saveLevel = 0;

    private void Awake()
    {
        Loading.SetActive(true);
    }
    public void init( )
    {
        player = Manager.Instance.ReturnPlayer().GetComponent<PlayerInfo>();
        Cursor.lockState = CursorLockMode.Confined;
        canvas.scaleFactor = UISize;
        ChangeBoxImage();
    }
    public void onUpdate()
    {
        HPMPcontrol();
        player = Manager.Instance.ReturnPlayer().GetComponent<PlayerInfo>();
        if (!Manager.Instance.playerAlive)
        {
            FadeIn(GameOver[0].GetComponent<TextMeshProUGUI>());
            FadeIn(GameOver[1].GetComponent<Image>());
            if (GameOver[1].GetComponent<Image>().color.a>=0.9f)
            {
                GameOver[1].GetComponent<Button>().interactable = true;
            }
            FadeIn(GameOver[2].GetComponent<TextMeshProUGUI>());
        }
       
        if(GameOver[0].GetComponent<TextMeshProUGUI>().alpha>=1)
        {
            Time.timeScale = 0;
        }

        if (saveLevel == Manager.Instance.stageUp.Length)
        {
            TextUpdate(EXP.GetChild(2).GetComponent<Text>(), "MAX");
            EXP.GetComponent<Image>().fillAmount = 1;
        }
        else if (saveLevel == Manager.Instance.PlayerLevel)
        {
            if (Manager.Instance.stageUp.Length >= player.level)
            {
                EXP.GetComponent<Image>().fillAmount = player.exp / Manager.Instance.stageUp[player.level - 1];
            }
        }
        else
        {
            saveLevel = Manager.Instance.PlayerLevel;
            EXP.GetComponent<Image>().fillAmount = 0;
            TextUpdate(EXP.GetChild(2).GetComponent<Text>(), saveLevel.ToString());
        }
    }

    public void HPMPcontrol()
    {
        if (player.GetComponent<PlayerInfo>().hp == 0)
        {
            saveHp = 0;
        }
        BarControll(player.GetComponent<PlayerInfo>().hp, ref saveHp, Hp);
        BarControll(player.GetComponent<PlayerInfo>().mp, ref saveMp, Mp);
    }
    public void TextUpdate(Text txt, string changeText)
    {
        txt.text = changeText;
    }
    public void FadeIn(TextMeshProUGUI txt)
    {
        Color targetcolor;
        targetcolor = txt.color;
        targetcolor.a = 255;
        txt.color = Color.Lerp(txt.color, targetcolor, Time.deltaTime * 0.001f);
    }
    public void FadeIn(Image img)
    {
        Color targetcolor;
        targetcolor = img.color;
        targetcolor.a = 255;
        img.color = Color.Lerp(img.color, targetcolor, Time.deltaTime * 0.001f);

    }
    public void BarControll(float hpmp,ref float save,Transform bar)
    {
        if (save == hpmp)
        {
            bar.GetComponent<ImgsFillDynamic>().SetValue(save / 100, false, 1);
        }
        else
        {
            bar.GetComponent<ImgsFillDynamic>().SetValue(save / 100, true, 1);
            save = hpmp;
        }
    }
    public void ChangeGunBox(Transform pos)
    {
        if (pos.GetChild(0).GetChild(0).GetComponent<Image>().enabled)
        {
            SelectedGun.GetComponent<RectTransform>().anchoredPosition = pos.GetComponent<RectTransform>().anchoredPosition;
        }
    }
    public void ChangeBoxImage()
    {
        for(int i=0;i <3;i++)
        {
            if (player.weapoNum[i].Num != -1)
            {
                GunBox[i].GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                GunBox[i].GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                   Manager.Instance._data.allSprites[1][player.weapoNum[i].Num];
            }
            else
            {
                GunBox[i].GetChild(0).GetChild(0).GetComponent<Image>().enabled=false;
            }

        }
    }
}

