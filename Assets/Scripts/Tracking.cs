using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Tracking : MonoBehaviour
{

    private StarterAssetsInputs _input;

    [SerializeField] private Transform capsuleEndPoint;
    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        Vector3 playerDirection = transform.TransformDirection(Vector3.forward);
        if(Physics.CapsuleCast(transform.position,capsuleEndPoint.position,0.28f, playerDirection, out RaycastHit hitInfo, 8, layerMask))
        {
            if(hitInfo.collider.TryGetComponent<Interactable>(out Interactable interactable))
            {
                //Debug.Log("hitinfo - " + hitInfo.collider.name);
                interactable.StartInteract(transform);
            }
            else
            {
                //Debug.Log("Hit But not Found interactable - " + hitInfo.collider.name);
            }
        }
    }
}
