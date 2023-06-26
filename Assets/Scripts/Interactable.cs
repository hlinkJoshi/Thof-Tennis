using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    private Transform playerTransform;
    bool isStart;
    [SerializeField] private Outline outline;
    [SerializeField] private string hexaColorCode = "#F8E36A";

    private void Start()
    {
        outline = GetComponent<Outline>();
        if(ColorUtility.TryParseHtmlString(hexaColorCode, out Color color))
        {
            outline.OutlineColor = color;
        }
        outline.enabled = false;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    private void Update()
    {
        if(isStart)
        {
            Vector3 playerToNpc = (transform.position - playerTransform.position).normalized;
            float look = Vector3.Dot(playerToNpc, playerTransform.forward);
            if(look <= 0.1f)
            {
                DisableInteract();
            }
        }
    }

    public void StartInteract(Transform temp)
    {
        if(!isStart)
        {
            playerTransform = temp;
            outline.enabled = true;
            isStart = true;
        }        
    }
    private void DisableInteract()
    {
        isStart = false;
        outline.enabled = false;
    }
}
