using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentMaker : MonoBehaviour
{
    private GameManager.ContentPiece current;
    private bool busy;

    public void DoContent(GameManager.ContentPiece content)
    {
        current = new GameManager.ContentPiece(content);
        busy = true;
    }

    public void UpdateContent()
    {
        //TODO:?

        //then
        busy = false;
    }

    public bool BusyWithContent()
    {
        return busy;
    }

    internal GameManager.ContentPiece GetModifiedContent()
    {
        return current;
    }
}
