using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    void Update()
    {
        transform.position = Manager.Instance.ReturnPlayer().transform.position;
    }
}
