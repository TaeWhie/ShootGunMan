using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControll : MonoBehaviour
{
    void Start()
    {
        #if UNITY_EDITOR
        Cursor.visible = false;
        #elif UNITY_ANDROID
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.enabled = false;
        #endif
    }

    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
        
    }
}
