using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part2Sequencer : Sequencer
{
    [SerializeField]
    private UIButton playGameButton = default;

    [SerializeField]
    private UIButton clearButton = default;

    [SerializeField]
    private BallThrower ballThrower = default;

    private bool buttonPressed;

    protected override IEnumerator Sequence()
    {
        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        yield return new WaitForSeconds(1f);
        playGameButton.Show();
        clearButton.Show();

        yield return new WaitUntil(() => buttonPressed);
        playGameButton.gameObject.SetActive(false);

        ballThrower.gameObject.SetActive(true);

        // WAIT FOR MINIGAME COMPLETION
        yield return new WaitForSeconds(8f);

        ballThrower.gameObject.SetActive(false);
        BuddyHandsController.Instance.ClearHandTargets();

        yield return new WaitForSeconds(1f);
        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[1]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        yield return new WaitUntil(() => FoodSpawner.Instance.NoFood);
    }

    public void OnButtonPressed()
    {
        buttonPressed = true;
    }
}
