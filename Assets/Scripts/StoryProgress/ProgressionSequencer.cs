using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSequencer : MonoBehaviour
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
        yield return DialogueHandler.Instance.PlayDialogueEvent(introEvents[0]);
        foreach (var button in initialButtons)
        {
            button.Show();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
