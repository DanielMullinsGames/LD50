using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowboatSequencer : Sequencer
{
    [SerializeField]
    private UIButton holidayButton = default;

    [SerializeField]
    private GameObject rowboatScene = default;

    private bool buttonPressed;

    protected override IEnumerator Sequence()
    {
        yield return new WaitForSeconds(0.5f);
        holidayButton.Show();

        yield return new WaitUntil(() => buttonPressed);
        rowboatScene.SetActive(true);

        holidayButton.gameObject.SetActive(false);
    }

    public void OnButtonPressed()
    {
        buttonPressed = true;
    }
}
