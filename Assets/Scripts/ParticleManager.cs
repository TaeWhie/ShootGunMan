using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject LevelupParticle;
    private int saveLevel=1;
    public void Start()
    {

    }
    public void OnUpdate()
    {

        if (saveLevel == Manager.Instance.stageUp.Length)
        {
           
        }
        else if (saveLevel == Manager.Instance.PlayerLevel)
        {
           
        }
        else
        {
            Instantiate(LevelupParticle, Manager.Instance.ReturnPlayer().transform.position,Manager.Instance.ReturnPlayer().transform.rotation);
            saveLevel = Manager.Instance.PlayerLevel;
        }
    }
}
