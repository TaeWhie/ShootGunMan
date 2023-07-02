using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenOak :AI
{
    public GreenOak(GameObject Enemy)
    {
        me = Enemy;
        e = me.GetComponent<Enemy>();
        mng = Manager.Instance;
    }

    public override void onUpdate()
    {
        base.onUpdate();
        mot.MovetoPlayer(me.transform,
        mng.ReturnPlayer().transform,e.totalSpeed);
    }
}
