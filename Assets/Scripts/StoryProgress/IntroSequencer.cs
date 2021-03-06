using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequencer : Sequencer
{
    [SerializeField]
    private List<UIButton> initialButtons = default;

    protected override IEnumerator Sequence()
    {
        initialButtons.ForEach(x => x.SetHidden());
        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => Input.anyKeyDown);

        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueHandler.Instance.NoDialoguePlaying);

        DeathHandler.Instance.MarkAsDead();

        DialogueHandler.Instance.AddDialogueEventToStack(dialogueEvents[1]);
        yield return new WaitUntil(() => CanProceed());
        foreach (var button in initialButtons)
        {
            button.Show();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitUntil(() => GameStatus.pressedClearFoodButton);
    }

    private bool CanProceed()
    {
        return DialogueHandler.Instance.NoDialoguePlaying && !GameStatus.shrunkWindow;
    }
}
