using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowboatSequencer : Sequencer
{
    [Header("Rowboat Sequence")]
    [SerializeField]
    private UIButton holidayButton = default;

    [SerializeField]
    private List<UIButton> buttonsToTempHide = new List<UIButton>();

    [SerializeField]
    private GameObject rowboatScene = default;

    [SerializeField]
    private Transform leftOar = default;

    [SerializeField]
    private Transform rightOar = default;

    private bool buttonPressed;

    protected override IEnumerator Sequence()
    {
        yield return new WaitForSeconds(0.5f);
        holidayButton.Show();

        yield return new WaitUntil(() => buttonPressed);
        GameStatus.didHoliday = true;
        rowboatScene.SetActive(true);
        buttonsToTempHide.ForEach(x => x.SetHidden());

        holidayButton.gameObject.SetActive(false);

        BuddyHandsController.Instance.SetLeftHandTarget(leftOar);
        BuddyHandsController.Instance.SetRightHandTarget(rightOar);

        yield return new WaitForSeconds(10f);
        AudioController.Instance.PlaySound2D("button_press_chunk", 1f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
        rowboatScene.SetActive(false);
        buttonsToTempHide.ForEach(x => x.Show());
        BuddyHandsController.Instance.ClearHandTargets();
    }

    public void OnButtonPressed()
    {
        buttonPressed = true;
    }
}
