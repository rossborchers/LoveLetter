using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ContentMaker ContentMaker;
    public ContentLog ContentLog;

    public List<ContentPiece> Content;

    public int ContentIndex = 0;

    public enum Sender
    {
        Player,
        Bot
    }

    [Serializable]
    public class ContentPiece
    {
        public Sender Sender;
        public string Expected;
        public string Actual;

        public ContentPiece(ContentPiece rhs)
        {
            this.Sender = rhs.Sender;
            this.Expected = rhs.Expected;
            this.Actual = rhs.Actual;
        }
    }

    private void Start()
    {
        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        int currentIndex = 0;
        int lastIndex = 0;
        while(currentIndex < Content.Count)
        {
            if(lastIndex < currentIndex)
            {
                ContentPiece currentPiece = Content[ContentIndex];

                ContentPiece modifiedPiece = null;
                if (currentPiece.Sender == Sender.Player)
                {
                    ContentMaker.DoContent(currentPiece);
                    while(ContentMaker.BusyWithContent())
                    {
                        ContentMaker.UpdateContent();
                        yield return null;
                    }
                    modifiedPiece = ContentMaker.GetModifiedContent();
                }
                ContentLog.Add(modifiedPiece);

                currentIndex++;
                lastIndex = currentIndex;
            }
            else
            {
                yield return null;
            }
        }

        //TODO: 
    }
}
