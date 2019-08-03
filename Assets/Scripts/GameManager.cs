using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<AudioSource> Sources = new List<AudioSource>();

    public TMP_Text Placeholder;
    public TMP_InputField Input;

    public List<string> Raw;
    public List<string> Sensored;

    private void Start()
    {
        if (Input)
        {
            Input.onValidateInput += ValidateInput;
        }

        List<char> newText = new List<char>() ;
        foreach( char c in Placeholder.text)
        {
            if (c == 13)
            {
                continue;
            }
            else
            {
                newText.Add(c);
            }
        }
        string t = new string(newText.ToArray());
        Placeholder.text = t;
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        Sources[Random.Range(0, Sources.Count)].Play();

        Debug.Log((0 + Placeholder.text[charIndex]) + " " + (0 + addedChar));
        char invalid = '\0';

        if (Placeholder.text[charIndex] == 10 || Placeholder.text[charIndex] == 13)
        {
            if(addedChar == 10 || addedChar == 13)
            {
                return addedChar;
            }
        }

        if (charIndex > Placeholder.text.Length - 1)
        {
            return invalid;
        }

        if (Placeholder.text[charIndex] == addedChar)
        {
            return addedChar;
        }
        else
        {
            return invalid;
        }
    }

    private void Update()
    {
        for(int i  = 0; i < Sensored.Count && i < Raw.Count; i++)
        {
            string raw = Raw[i];
            string sensored = Sensored[i];

            if (Input.text.Contains(raw))
            {
                //int index =Input.text.index(raw);
                Input.text = Input.text.Replace(raw, sensored);
                Placeholder.text = Placeholder.text.Replace(raw, sensored);

                Input.caretPosition += sensored.Length - raw.Length;
            }
        }
    }
}
