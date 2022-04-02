using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueEvent", menuName = "DialogueEvent", order = 1)]
public class DialogueEvent : ScriptableObject
{
    public List<string> lines = new List<string>();
}
