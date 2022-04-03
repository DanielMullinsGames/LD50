using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class QuitButtonHandler : ManagedBehaviour
{
    [SerializeField]
    private List<DialogueEvent> dialogueEvents = new List<DialogueEvent>();

    [SerializeField]
    private Transform alternateButtonPosMarker = default;

    [SerializeField]
    private UIButton quitButton = default;

    private int evadeQuitProgress = 0;
    private Vector2 originalButtonPos;

    private void Start()
    {
        originalButtonPos = quitButton.transform.position;
    }

    public void OnQuitPressed()
    {
        if (DeathHandler.Instance.Dead)
        {
            Application.Quit();
        }

        switch (evadeQuitProgress)
        {
            case 0:
                quitButton.SetCollisionEnabled(false);
                Tween.Position(quitButton.transform, alternateButtonPosMarker.position, 0.2f, 0f, Tween.EaseInOut, completeCallback: () =>
                {
                    quitButton.SetCollisionEnabled(true);
                });

                DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[0]);
                break;
            case 1:
                quitButton.SetCollisionEnabled(false);
                Tween.Position(quitButton.transform, originalButtonPos, 0.2f, 0f, Tween.EaseInOut, completeCallback: () =>
                {
                    quitButton.SetCollisionEnabled(true);
                });

                DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[1]);
                break;
            case 2:
                quitButton.SetCollisionEnabled(false);
                Screen.SetResolution(300, 160, false);
                CustomCoroutine.WaitThenExecute(0.1f, () =>
                {
                    CustomCoroutine.WaitOnConditionThenExecute(() => MousePosChecker.MouseOverGameWindow, () => Screen.SetResolution(900, 480, false));
                    DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[2]);
                    quitButton.SetCollisionEnabled(true);
                });
                break;
            case 3:
                Application.Quit();
                break;
        }
        
        evadeQuitProgress++;
    }
}
