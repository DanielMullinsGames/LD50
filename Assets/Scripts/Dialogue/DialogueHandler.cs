using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : Singleton<DialogueHandler>
{
    [SerializeField]
    private DialogueText dialogueText = default;

    [SerializeField]
    private BuddyMouth mouth = default;

    private void Start()
    {
        dialogueText.DisplayCharacter += OnDisplayCharacter;
        dialogueText.CompletedLine += OnMessageEnded;
    }

    public IEnumerator PlayDialogueEvent(DialogueEvent dialogueEvent)
    {
        foreach (string line in dialogueEvent.lines)
        {
            dialogueText.PlayMessage(line);
            yield return new WaitUntil(() => !dialogueText.PlayingMessage);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        dialogueText.Clear();
    }

    private void OnDisplayCharacter(string message, int index)
    {
        mouth.ToggleOpen();
        AudioController.Instance.PlaySound2D("buddy_voice_1", pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.VerySmall));
    }

    private void OnMessageEnded()
    {
        mouth.SetOpen(false);
    }
}
