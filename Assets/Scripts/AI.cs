using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI 
{
    protected AI_MovetoTarget mot = new();
    protected GameObject me;
    protected Enemy e;
    protected Manager mng;
    public virtual void onUpdate()
    {

    }
}
