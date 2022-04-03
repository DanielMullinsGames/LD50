using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part3Sequencer : Sequencer
{
    [SerializeField]
    private UIButton questButton = default;

    [SerializeField]
    private UIButton holidayButton = default;

    [SerializeField]
    private RPGManager rpgManager = default;

    private bool buttonPressed;

    protected override IEnumerator Sequence()
    {
        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        yield return new WaitForSeconds(1f);
        questButton.Show();

        yield return new WaitUntil(() => buttonPressed);
        questButton.gameObject.SetActive(false);
        rpgManager.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[1]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        yield return new WaitUntil(() => rpgManager.GameOver);
        yield return new WaitForSeconds(3f);

        GameStatus.canUseHolidayCode = true;
        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[2]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        yield return new WaitUntil(() => GameStatus.usedHolidayCode);
        holidayButton.Show();
    }

    public void OnButtonPressed()
    {
        buttonPressed = true;
    }
}
