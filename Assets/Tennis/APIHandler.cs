using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIHandler : MonoBehaviour
{
    IEnumerator UpdateScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("Aces", 1);
        form.AddField("Breakpointswon", 1);
        form.AddField("Receivingpointswon", 1);
        form.AddField("Winners", 1);
        form.AddField("Unforcederrors", 1);
        form.AddField("Totalpointswon", 1);
        form.AddField("Fastestserve(kmh)", 1);

        using (UnityWebRequest request = UnityWebRequest.Post("url", form))
        {
            yield return request.SendWebRequest();
            if(request.result  == UnityWebRequest.Result.Success)
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
