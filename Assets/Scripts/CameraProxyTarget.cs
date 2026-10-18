using System;
using UnityEngine;

public class CameraProxyTarget : MonoBehaviour
{
    [SerializeField]
    private float zPos = 0f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
    }
}
