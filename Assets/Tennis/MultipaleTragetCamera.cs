using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MultipaleTragetCamera : MonoBehaviour
{
    [SerializeField] List<Transform> objectList;
    Bounds objectBounds;

    float minZoom = 60f;
    float maxZoom = 90f;
    float zoomLimit = 100f;

    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void LateUpdate()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, LargeDistance() / zoomLimit);
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, newZoom, Time.deltaTime);
    }

    float LargeDistance()
    {
        objectBounds = new Bounds(objectList[0].position, Vector3.zero);
        for (int i = 0; i < objectList.Count; i++)
        {
            objectBounds.Encapsulate(objectList[i].position);
        }
        return objectBounds.size.x;
    }
}
