using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text content;

    public void SetContent(string text)
    {
        content.text = text;
    }

    public void OnClickYes()
    {
        // TODO
    }

    public void OnClickNo()
    {
        // TODO
    }

}
