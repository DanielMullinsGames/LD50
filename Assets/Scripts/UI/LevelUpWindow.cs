using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpWindow : MonoBehaviour
{
    public bool Choosing => window.activeInHierarchy;

    public enum LevelUpChoice
    {
        MaxHealth,
        Attack,
        CritChance,
        CritMulti,
        NUM_OPTIONS,
    }

    [SerializeField]
    private GameObject window = default;

    [SerializeField]
    private RPGEntity player = default;

    [SerializeField]
    private Text choiceButton1 = default;

    [SerializeField]
    private Text choiceButton2 = default;

    private LevelUpChoice choice1;
    private LevelUpChoice choice2;

    public void ShowLevelUp()
    {
        AudioController.Instance.PlaySound2D("awww", 0.5f);
        window.SetActive(true);
        choice1 = (LevelUpChoice)Random.Range(0, (int)LevelUpChoice.NUM_OPTIONS);
        while (choice2 == choice1)
        {
            choice2 = (LevelUpChoice)Random.Range(0, (int)LevelUpChoice.NUM_OPTIONS);
        }
        ShowTextForChoice(choiceButton1, choice1);
        ShowTextForChoice(choiceButton2, choice2);
    }

    private void ShowTextForChoice(Text text, LevelUpChoice choice)
    {
        switch (choice)
        {
            case LevelUpChoice.Attack:
                text.text = "Attack +1";
                break;
            case LevelUpChoice.CritChance:
                text.text = "Crit Chance +10%";
                break;
            case LevelUpChoice.CritMulti:
                text.text = "Crit Multiplier +1";
                break;
            case LevelUpChoice.MaxHealth:
                text.text = "Max Health +5";
                break;
        }
    }

    private void ApplyChoice(LevelUpChoice choice)
    {
        switch (choice)
        {
            case LevelUpChoice.Attack:
                player.attackPower += 1;
                break;
            case LevelUpChoice.CritChance:
                player.critChance += 0.1f;
                break;
            case LevelUpChoice.CritMulti:
                player.critMultiplier += 1;
                break;
            case LevelUpChoice.MaxHealth:
                player.maxHealth += 5;
                break;
        }
    }

    public void OnButtonPressed(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                player.health = player.maxHealth;
                player.UpdateHealthBar();
                break;
            case 1:
                ApplyChoice(choice1);
                player.UpdateHealthBar(true);
                break;
            case 2:
                ApplyChoice(choice2);
                player.UpdateHealthBar(true);
                break;
        }

        window.SetActive(false);
        AudioController.Instance.PlaySound2D("promo_success", 0.5f);
    }
}
