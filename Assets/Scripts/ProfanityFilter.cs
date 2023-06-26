using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class ProfanityFilter : MonoBehaviour
{
    // UI
    //public Text textDisplay;
    //public InputField inputText;

    // Block list
    public TextAsset textAssetBlocklist;
    public string[] strBlocklist;

    public static ProfanityFilter instance;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        strBlocklist = textAssetBlocklist.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public string CheckInput(string stringToCheck)
    {
        // Call profanityCheck to filter the input text
        string newText = profanityCheck(stringToCheck);
        return newText;
    }

    string profanityCheck(string strToCheck)
    {
        for (int i = 0; i < strBlocklist.Length; i++)
        {
            string profanity = strBlocklist[i].Trim();
            // Create a regular expression pattern to match the profanity as a whole word
            string pattern = @"\b" + Regex.Escape(profanity) + @"\b";
            strToCheck = Regex.Replace(strToCheck, pattern, "****", RegexOptions.IgnoreCase);
        }
        return strToCheck;
    }
}
