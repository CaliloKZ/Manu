using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueText", menuName = "DialogueText")]
public class DialogueText : ScriptableObject
{
    [TextArea]
    public string text;
    public Color fontColor;
    public List<string> voices = new List<string>();
}
