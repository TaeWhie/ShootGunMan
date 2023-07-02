using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 cameraPosition;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private Transform playerTransform;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + cameraPosition,
                                  Time.deltaTime * cameraMoveSpeed);
    }
}
