using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Sharing : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CopyToClipboardAndShare(string textToCopy);
    //----------------------------------------

    //private string appId = "1335095564084647";
    //string Picture = "https://itocksdev.blob.core.windows.net/itocks/itock_15";
    //string Link = "https://play.google.com/store/apps/details?id=com.netmarble.metaworld";
    //string Caption = "Story";
    //string Description = "Description of the story";
    //string Name = "Name Is required";

    string gameUrl = "http://52.21.127.189/Unity";

    public void FBShare()
    {
        //Application.OpenURL("https://www.facebook.com/dialog/feed?" + "app_id=" + appId + "&link=" +
        //                     Link + "&picture=" + Picture + "&name=" + ReplaceSpace(Name) + "&caption=" +
        //                     ReplaceSpace(Caption) + "&description=" + ReplaceSpace(Description) +
        //                     "&redirect_uri=https://facebook.com/"
        //                     );

        Application.OpenURL("https://www.facebook.com/sharer.php?u=" + gameUrl);
    }
    string ReplaceSpace(string val)
    {
        return val.Replace(" ", "%20");
    }

    //----------------------------------------

    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    //private const string TWEET_LANGUAGE = "en";
    public static string descriptionParam;
    //private string appStoreLink = "http://www.YOUROWNAPPLINK.com";
    public void TwitterShare()
    {
        string nameParameter = "THOF Metaverse, enjoy Metaverse Excperiance with friends";
        Application.OpenURL(TWITTER_ADDRESS +
           "?text=" + WWW.EscapeURL(nameParameter + "\n" + descriptionParam + "\n" + "\n" + gameUrl));
    }

    //----------------------------------------
    public Text textToCopy;
    public void CopyLink()
    {
        string str = textToCopy.text;
        SSTools.ShowMessage("Copied link to clipboard!", SSTools.Position.top, SSTools.Time.twoSecond);
        CopyToClipboardAndShare(str);
    }

    //----------------------------------------

    //public void OpenDiscord()
    //{
    //    Application.OpenURL("https://www.google.com/");
    //}

    public void OpenTW()
    {
        string twittershare = "https://twitter.com/messages/";
        Application.OpenURL(twittershare);
    }

    public void OpenFB()
    {
        string facebookshare = "https://www.facebook.com/friends/list";
        Application.OpenURL(facebookshare);
    }

    public void OpenDiscord()
    {
        string facebookshare = "https://discord.com/channels/@me";
        Application.OpenURL(facebookshare);
    }
}
