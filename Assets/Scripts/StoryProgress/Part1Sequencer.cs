using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part1Sequencer : ManagedBehaviour
{
    [SerializeField]
    private List<DialogueEvent> events = default;

    private void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(3f);
        DialogueHandler.Instance.AddDialogueEventToStack(events[0]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);
    }
}
