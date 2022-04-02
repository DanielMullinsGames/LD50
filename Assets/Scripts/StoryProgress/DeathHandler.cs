using UnityEngine;

public class DeathHandler : Singleton<DeathHandler>
{
    public bool Dead { get; private set; }

    [SerializeField]
    private GameObject aliveBuddy = default;

    [SerializeField]
    private GameObject deadBuddy = default;

    [SerializeField]
    private GameObject aliveBuddyHat = default;

    [SerializeField]
    private GameObject deadBuddyHat = default;

    [SerializeField]
    private GameObject firstSequencer = default;

    [SerializeField]
    private DialogueEvent hatDialogue = default;

    [Header("Debug")]
    [SerializeField]
    private bool debugDead = false;

    [SerializeField]
    private bool debugDontStartIntro = false;

    private void Start()
    {
        if (DeathIsMarked())
        {
            Dead = true;
            deadBuddy.SetActive(true);
            aliveBuddy.SetActive(false);
        }
        else
        {
            deadBuddy.SetActive(false);
            aliveBuddy.SetActive(true);

#if UNITY_EDITOR
            if (debugDontStartIntro)
            {
                return;
            }
#endif

            firstSequencer.SetActive(true);
        }
    }

    public void SetLastWords(string words)
    {

    }

    public void AddHat()
    {
        aliveBuddyHat.SetActive(true);
        deadBuddyHat.SetActive(true);
        CustomCoroutine.WaitThenExecute(0.5f, () => DialogueHandler.Instance.AddDialogueEventToStack(hatDialogue));
    }

    public void MarkAsDead()
    {

    }

    private bool DeathIsMarked()
    {
#if UNITY_EDITOR
        if (debugDead)
        {
            return true;
        }
#endif
        else
        {
            return false;
        }
    }

    private void ClearDeath()
    {

    }
}
