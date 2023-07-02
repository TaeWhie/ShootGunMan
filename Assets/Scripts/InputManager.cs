using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputManager
{
    public Action keyaction = null;

    public void onUpdate()
    {
        if (Input.anyKey == false) return;
        if(keyaction!=null)
        {
            keyaction.Invoke();
        }
    }
}
