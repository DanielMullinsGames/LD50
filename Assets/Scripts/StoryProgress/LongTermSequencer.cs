using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongTermSequencer : Sequencer
{
    private Vector2 lastCursorPos;
    private float timeSinceMovedCursor;

    [SerializeField]
    private DialogueEvent finalDeathDialogue = default;

    [SerializeField]
    private BuddyEyes eyes = default;

    private bool playedHolidayDialogue1;
    private bool playedHolidayDialogue2;

    protected override IEnumerator Sequence()
    {
        yield break;
    }

    public override void ManagedUpdate()
    {
        if (Vector3.Distance(lastCursorPos, InteractionCursor.Instance.transform.position) > 0.01f)
        {
            TryPlayDialogueEventForTime();
            timeSinceMovedCursor = 0f;

            if (DialogueHandler.Instance.NoDialoguePlaying)
            {
                eyes.SetEmotion(Emotion.Neutral);
            }
        }
        else
        {
            timeSinceMovedCursor += Time.deltaTime;

            if (timeSinceMovedCursor > 60f)
            {
                if (DialogueHandler.Instance.NoDialoguePlaying)
                {
                    eyes.SetEmotion(Emotion.Sleeping);
                }
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
        if (GameClock.Instance.Minutes > 30 && GameClock.Instance.Hours < 24 && !playedHolidayDialogue1)
        {
            playedHolidayDialogue1 = true;
            DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        }
        else if (GameClock.Instance.Hours >= 24 && !playedHolidayDialogue2)
        {
            playedHolidayDialogue2 = true;
            DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[5]);
        }
        else if (GameClock.Instance.Hours >= 48)
        {
            StartCoroutine(Death());
        }
        else if (GameClock.Instance.Hours >= 46)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[7]);
        }
        else if (GameClock.Instance.Hours >= 32)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[6]);
        }
        else if (GameClock.Instance.Hours >= 8)
        {
            if (!playedHolidayDialogue2)
            {
                DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[4]);
            }
        }
        else if (GameClock.Instance.Hours >= 4)
        {
            if (!playedHolidayDialogue2)
            {
                DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[3]);
            }
        }
        else if (GameClock.Instance.Hours >= 2)
        {
            if (!playedHolidayDialogue2)
            {
                DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[2]);
            }
        }
        else if (GameClock.Instance.Hours >= 1)
        {
            if (!playedHolidayDialogue2)
            {
                DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[1]);
            }
        }
    }
}
