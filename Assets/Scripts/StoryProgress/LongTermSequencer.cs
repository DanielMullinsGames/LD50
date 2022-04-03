using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongTermSequencer : Sequencer
{
    private Vector2 lastCursorPos;
    private float timeSinceMovedCursor;
    private bool playedAbandonedDialogue;

    [SerializeField]
    private DialogueEvent finalDeathDialogue = default;

    protected override IEnumerator Sequence()
    {
        yield break;
    }

    public override void ManagedUpdate()
    {
        if (Vector3.Distance(lastCursorPos, InteractionCursor.Instance.transform.position) > 0.01f)
        {
            timeSinceMovedCursor = 0f;
            playedAbandonedDialogue = false;

            TryPlayDialogueEventForTime();
        }
        else
        {
            timeSinceMovedCursor += Time.deltaTime;

            if (timeSinceMovedCursor > 120f && !playedAbandonedDialogue)
            {
                playedAbandonedDialogue = true;
                TryPlayAbandonedDialogue();
            }
        }

        lastCursorPos = InteractionCursor.Instance.transform.position;
    }

    private IEnumerator Death()
    {
        enabled = false;

        DialogueHandler.Instance.AddDialogueEventToStack(finalDeathDialogue);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(1f);
        DeathHandler.Instance.Dead = true;
        BuddyAger.Instance.Die();
    }

    private void TryPlayDialogueEventForTime()
    {
        if (GameClock.Instance.Hours >= 48)
        {
            StartCoroutine(Death());
        }
        else if (GameClock.Instance.Hours >= 46)
        {

        }
        else if (GameClock.Instance.Hours >= 32)
        {

        }
        else if (GameClock.Instance.Hours >= 24)
        {

        }
        else if (GameClock.Instance.Hours >= 12)
        {

        }
        else if (GameClock.Instance.Hours >= 8)
        {

        }
        else if (GameClock.Instance.Hours >= 6)
        {

        }
        else if (GameClock.Instance.Hours >= 4)
        {

        }
        else if (GameClock.Instance.Hours >= 2)
        {

        }
        else if (GameClock.Instance.Hours >= 1)
        {

        }
        else if (GameClock.Instance.Minutes > 35)
        {

        }
        else if (GameClock.Instance.Minutes > 20)
        {

        }
    }

    private void TryPlayAbandonedDialogue()
    {

    }
}
