using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Validator", menuName = "Validator")]
public class Validator : TMP_InputValidator
{
    public string Expected;

    public override char Validate(ref string text, ref int pos, char ch)
    {
        char invalid = '\0';
        if (pos > Expected.Length-1)
        {
            return invalid;
        }

       if(Expected[pos] == ch)
       {
           return ch;
       }
       else
       {
            return invalid;
       }
    }
}
