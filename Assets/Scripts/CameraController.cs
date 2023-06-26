using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 0.25f;
    private CinemachineComposer composer;

    [SerializeField]
    private FixedJoystick cameraController;

    [DllImport("__Internal")]
    private static extern bool IsMobile();
    [DllImport("__Internal")]
    private static extern bool IsMobileModified();

    public bool isMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobile();
#endif
        return false;
    }
    public bool IsMobileCheck()
    {

#if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobileModified();
#endif
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        composer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical;
        if (IsMobileCheck())
        {
            vertical = cameraController.Vertical * Time.deltaTime * 4f;
            composer.m_TrackedObjectOffset.y += vertical;
        }
        else
        {
            vertical = Input.GetAxis("Mouse Y") * sensitivity *0.5f ;
            if (Input.GetMouseButton(0) && Mathf.Abs(vertical) > 0.05f)
            {
                composer.m_TrackedObjectOffset.y += vertical;
            }
        }
        composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, -3, 3);

    }
}
