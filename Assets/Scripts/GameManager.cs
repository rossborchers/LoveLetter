using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TMP_Text Placeholder;
    public TMP_InputField Input;

    private void Start()
    {
        if (Input)
        {
            Input.onValidateInput += ValidateInput;
        }
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
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
}
