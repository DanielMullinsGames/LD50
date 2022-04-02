using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSequencer : MonoBehaviour
{
    [SerializeField]
    private List<DialogueEvent> introEvents = default;

    private void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(1f);
        yield return DialogueHandler.Instance.PlayDialogueEvent(introEvents[0]);
    }
}
