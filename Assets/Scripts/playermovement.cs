using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class playermovement : MonoBehaviourPunCallbacks
{
    void Start()
    {
        transform.GetChild(3).GetChild(0).GetComponent<Text>().text = this.photonView.Controller.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float xx = Input.GetAxis("Horizontal") * 10 * Time.deltaTime;
            float zz = Input.GetAxis("Vertical") * 10 * Time.deltaTime;
            transform.Translate(xx, 0, zz);
        }
    }
}
