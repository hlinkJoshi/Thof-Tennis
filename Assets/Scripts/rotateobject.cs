using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateobject : MonoBehaviour
{
    float x, y;
    float val = 0f;
    Vector3 newVec = Vector3.up + Vector3.right;
    public void Updated()
    {
        val = Time.deltaTime * Mathf.Rad2Deg * 0.1f;
        x = Input.GetAxis("Mouse X") * val;
        y = Input.GetAxis("Mouse Y") * val;

        transform.RotateAroundLocal(Vector3.up,-x);
        transform.RotateAroundLocal(Vector3.right, y);
    }
}
