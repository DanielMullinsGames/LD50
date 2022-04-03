using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part2Sequencer : Sequencer
{
    protected override IEnumerator Sequence()
    {
        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        // REVEAL BUTTON

        // WAIT FOR MINIGAME COMPLETION
    }
}
