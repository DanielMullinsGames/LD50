using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sequencer : ManagedBehaviour
{
    [SerializeField]
    protected List<DialogueEvent> dialogueEvents = default;

    [SerializeField]
    private float waitBeforeNext = 0f;

    [SerializeField]
    private GameObject activateWhenComplete = default;

    private void Start()
    {
        StartCoroutine(SequenceThenActivateNext());
    }

    private IEnumerator SequenceThenActivateNext()
    {
        yield return Sequence();

        yield return new WaitForSeconds(waitBeforeNext);
        if (activateWhenComplete != null)
        {
            activateWhenComplete.SetActive(true);
        }
    }

    protected virtual IEnumerator Sequence() { yield break; }
}
