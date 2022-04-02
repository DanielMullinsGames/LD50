﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequencer : MonoBehaviour
{
    [SerializeField]
    private List<DialogueEvent> introEvents = default;

    [SerializeField]
    private List<UIButton> initialButtons = default;

    private void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        initialButtons.ForEach(x => x.SetHidden());
        yield return new WaitForSeconds(1f);
        DialogueHandler.Instance.AddDialogueEventToStack(introEvents[0]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        DeathHandler.Instance.MarkAsDead();

        DialogueHandler.Instance.AddDialogueEventToStack(introEvents[1]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);
        foreach (var button in initialButtons)
        {
            button.Show();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
