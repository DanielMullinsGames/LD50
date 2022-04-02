using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosChecker : ManagedBehaviour
{
    bool MouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || 
                Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }

    [SerializeField]
    private List<DialogueEvent> dialogueEvents = new List<DialogueEvent>();

    private int dialogueEventIndex = 0;
    private float secondsMouseOutOfWindow = 0f;
    private bool playedDialogueOutOfWindow = true;

    public override void ManagedUpdate()
    {
        if (MouseOverGameWindow)
        {
            secondsMouseOutOfWindow = 0f;
            playedDialogueOutOfWindow = false;
        }
        else
        {
            secondsMouseOutOfWindow += Time.deltaTime;

            if (secondsMouseOutOfWindow > 1f + (dialogueEventIndex * 0.25f))
            {
                if (dialogueEventIndex < dialogueEvents.Count && !playedDialogueOutOfWindow)
                {
                    DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[dialogueEventIndex]);
                    playedDialogueOutOfWindow = true;
                    dialogueEventIndex++;
                }

            }
        }
    }
}
