using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlockerSequencer : Sequencer
{
    [SerializeField]
    private GameObject screenBlocker = default;

    private bool mouseExitReentered = false;

    protected override IEnumerator Sequence()
    {
        screenBlocker.SetActive(true);
        float startTime = Time.time;

        StartCoroutine(WaitForMouseExitReenter());
        yield return new WaitUntil(() => Time.time - startTime > 10f || mouseExitReentered);

        screenBlocker.SetActive(false);

        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);
    }

    private IEnumerator WaitForMouseExitReenter()
    {
        yield return new WaitUntil(() => !MousePosChecker.MouseOverGameWindow);
        yield return new WaitUntil(() => MousePosChecker.MouseOverGameWindow);
        mouseExitReentered = true;
    }
}
