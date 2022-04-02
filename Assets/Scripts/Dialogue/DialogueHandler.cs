﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : Singleton<DialogueHandler>
{
    public bool NoDialoguePlaying => eventStack.Count == 0;
    public DialogueEvent CurrentEvent => NoDialoguePlaying ? null : eventStack.Peek();

    [SerializeField]
    private DialogueText dialogueText = default;

    [SerializeField]
    private BuddyMouth mouth = default;

    [SerializeField]
    private DialogueEvent resumeLines = default;

    private Stack<DialogueEvent> eventStack = new Stack<DialogueEvent>();
    private List<DialogueEvent> playedEvents = new List<DialogueEvent>();

    private void Start()
    {
        dialogueText.DisplayCharacter += OnDisplayCharacter;
        dialogueText.CompletedLine += OnMessageEnded;
    }

    public void AddDialogueEventToStack(DialogueEvent dialogueEvent)
    {
        if (!playedEvents.Contains(dialogueEvent))
        {
            if (eventStack.Count == 0 || eventStack.Peek().interruptBehaviour != DialogueEvent.InterruptBehaviour.Uninterruptable)
            {
                StartCoroutine(PlayDialogueEvent(dialogueEvent));
            }
        }
    }

    private IEnumerator PlayDialogueEvent(DialogueEvent dialogueEvent)
    {
        playedEvents.Add(dialogueEvent);

        eventStack.Push(dialogueEvent);
        dialogueText.Clear();

        bool continueToNextLine = true;
        for (int i = 0; i < dialogueEvent.lines.Count; i++)
        {
            if (eventStack.Peek() != dialogueEvent)
            {
                while (eventStack.Peek() != dialogueEvent)
                {
                    yield return new WaitForEndOfFrame();
                }

                switch (dialogueEvent.interruptBehaviour)
                {
                    case DialogueEvent.InterruptBehaviour.Skip:
                        continueToNextLine = false;
                        break;
                    case DialogueEvent.InterruptBehaviour.ResumeAfter:
                        i = Mathf.Max(i - 1, 0);
                        dialogueText.PlayMessage(resumeLines.lines[Random.Range(0, resumeLines.lines.Count)]);
                        yield return new WaitUntil(() => !dialogueText.PlayingMessage);
                        break;
                }
            }

            var line = dialogueEvent.lines[i];
            if (continueToNextLine)
            {
                dialogueText.PlayMessage(line);
                yield return new WaitUntil(() => !dialogueText.PlayingMessage || eventStack.Peek() != dialogueEvent);
                yield return new WaitForSeconds(0.1f);
            }
        }

        eventStack.Pop();
        if (eventStack.Count == 0)
        {
            yield return new WaitForSeconds(0.5f);
            dialogueText.Clear();
        }
    }

    private void OnDisplayCharacter(string message, int index)
    {
        mouth.ToggleOpen();
        AudioController.Instance.PlaySound2D("buddy_voice_1", pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.VerySmall), 
            repetition: new AudioParams.Repetition(0.05f));
    }

    private void OnMessageEnded()
    {
        mouth.SetOpen(false);
    }
}
