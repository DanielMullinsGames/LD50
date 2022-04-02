using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueEvent", menuName = "DialogueEvent", order = 1)]
public class DialogueEvent : ScriptableObject
{
    public enum InterruptBehaviour
    {
        Skip,
        ResumeAfter,
        Uninterruptable,
    }

    public InterruptBehaviour interruptBehaviour = InterruptBehaviour.Skip;
    public List<string> lines = new List<string>();
}
