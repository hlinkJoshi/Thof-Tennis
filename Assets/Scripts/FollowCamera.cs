using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if(mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
        }
    }
}
