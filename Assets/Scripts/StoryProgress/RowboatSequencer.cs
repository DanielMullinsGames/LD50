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

        var rowboatAudio = AudioController.Instance.PlaySound2D("rowboat", 0.75f);

        GameStatus.didHoliday = true;
        rowboatScene.SetActive(true);
        buttonsToTempHide.ForEach(x => x.SetHidden());

        holidayButton.gameObject.SetActive(false);

        BuddyHandsController.Instance.SetLeftHandTarget(leftOar);
        BuddyHandsController.Instance.SetRightHandTarget(rightOar);

        yield return new WaitForSeconds(5f);

        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(5f);

        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[1]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(5f);

        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[2]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(12f);

        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[3]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(7.5f);

        Destroy(rowboatAudio.gameObject);
        AudioController.Instance.PlaySound2D("button_press_chunk", 1f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
        rowboatScene.SetActive(false);
        BuddyHandsController.Instance.ClearHandTargets();

        yield return new WaitForSeconds(3f);
        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[4]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        yield return new WaitForSeconds(3f);
        buttonsToTempHide.ForEach(x => x.Show());
    }

    public void OnButtonPressed()
    {
        buttonPressed = true;
    }
}
