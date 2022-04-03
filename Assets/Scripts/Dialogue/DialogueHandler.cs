using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : Singleton<DialogueHandler>
{
    public bool NoDialoguePlaying => eventStack.Count == 0;
    public DialogueEvent CurrentEvent => NoDialoguePlaying ? null : eventStack.Peek();

    [SerializeField]
    private DialogueText dialogueText = default;

    [SerializeField]
    private BuddyEyes eyes = default;

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
        dialogueText.EmotionChange += OnEmotionChange;
    }

    public void AddDialogueEventToStack(DialogueEvent dialogueEvent)
    {
        if (!playedEvents.Contains(dialogueEvent) && !DeathHandler.Instance.Dead)
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
                yield return new WaitWhile(() => eventStack.Peek() != dialogueEvent);

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

        yield return new WaitWhile(() => eventStack.Peek() != dialogueEvent);
        eventStack.Pop();
        if (eventStack.Count == 0)
        {
            yield return new WaitForSeconds(0.5f);
            if (eventStack.Count == 0)
            {
                dialogueText.Clear();
            }
        }
    }

    private void OnEmotionChange(Emotion emotion)
    {
        eyes.SetEmotion(emotion);
        mouth.SetEmotion(emotion);
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
