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
        switch (evadeQuitProgress)
        {
            case 0:
                Tween.Position(quitButton.transform, alternateButtonPosMarker.position, 0.2f, 0f, Tween.EaseInOut);
                break;
            case 1:
                Tween.Position(quitButton.transform, originalButtonPos, 0.2f, 0f, Tween.EaseInOut);
                break;
            case 2:
                Screen.SetResolution(300, 160, false);
                CustomCoroutine.WaitThenExecute(0.1f, () =>
                {
                    CustomCoroutine.WaitOnConditionThenExecute(() => MousePosChecker.MouseOverGameWindow, () => Screen.SetResolution(900, 480, false));
                });
                break;
            case 3:
                Application.Quit();
                break;
        }
        evadeQuitProgress++;
    }
}
