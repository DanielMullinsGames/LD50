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

        PlayLine("Don't close this window.");
    }

    public void PlayLine(string line)
    {
        dialogueText.PlayMessage(line);
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
