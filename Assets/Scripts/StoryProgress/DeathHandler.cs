using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class DeathHandler : Singleton<DeathHandler>
{
    public bool Dead { get; set; }

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

    [SerializeField]
    private List<UIButton> holidayHideUIButtons = default;

    [SerializeField]
    private GameObject holidayDeadScene = default;

    [Header("Debug")]
    [SerializeField]
    private bool debugDead = false;

    [SerializeField]
    private bool debugDeadHoliday = false;

    [SerializeField]
    private bool debugDontStartIntro = false;

    private string DataFilePath => Application.dataPath + "/RuntimeInitialize.txt";
    private string PersistentDataFilePath => Application.persistentDataPath + "/Player-prev (1).txt";

    private void Start()
    {
#if UNITY_EDITOR
        if (debugDeadHoliday)
        {
            PlayerPrefs.SetInt("Holiday", 1);
        }
#endif

        if (DeathIsMarked())
        {
            Dead = true;
            deadBuddy.SetActive(true);
            aliveBuddy.SetActive(false);

            if (PlayerPrefs.GetInt("Holiday") == 1)
            {
                holidayHideUIButtons.ForEach(x => x.SetHidden());
                holidayDeadScene.SetActive(true);
                AudioController.Instance.PlaySound2D("rowboat", 0.75f);
            }
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

    public void AddHat()
    {
        CustomCoroutine.FlickerSequence(() => SetHatOn(true), () => SetHatOn(false), true, true, 0.1f, 4);
        CustomCoroutine.WaitThenExecute(0.5f, () => DialogueHandler.Instance.AddDialogueEventToStack(hatDialogue));
    }

    public void MarkAsDead()
    {
#if !UNITY_EDITOR
        PlayerPrefs.SetInt("Dead", 1);
        File.Create(DataFilePath);
        File.Create(PersistentDataFilePath);
#endif
    }

    private void SetHatOn(bool on)
    {
        aliveBuddyHat.SetActive(on);
        deadBuddyHat.SetActive(on);
    }

    private bool DeathIsMarked()
    {
#if UNITY_EDITOR
        if (debugDead)
        {
            return true;
        }
        else
#endif
        {
            return PlayerPrefs.GetInt("Dead") == 1 || File.Exists(DataFilePath) || File.Exists(PersistentDataFilePath);
        }
    }

    [Button("Clear Death Files")]
    private void ClearDeath()
    {
        File.Delete(DataFilePath);
        File.Delete(PersistentDataFilePath);
        PlayerPrefs.DeleteAll();
    }
}
