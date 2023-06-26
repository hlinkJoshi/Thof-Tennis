using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapName : MonoBehaviour
{
    public GameObject Uihandler_obj;
    private UIhandler uihandler;
    // Start is called before the first frame update
    void Start()
    {
        uihandler = Uihandler_obj.transform.GetComponent<UIhandler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == api.nickName)
        {
            switch (transform.name)
            {
                case "Tutorial1":
                    uihandler.Map_Location = 1;
                    break;

                case "Tutorial5":
                    uihandler.Map_Location = 2;
                    break;

                case "Tutorial6":
                    uihandler.Map_Location = 3;
                    break;

                case "Tutorial4":
                    uihandler.Map_Location = 4;
                    break;
            }
        }
    }
    //void OnTriggerExit(Collider other)
    //{
    //    if (other.name == api.nickName)
    //    {
    //        switch (transform.name)
    //        {
    //            case "Tutorial4":
    //                uihandler.Map_Location = 2;
    //                break;
    //        }
    //    }
    //}
}
