using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class RPGManager : Singleton<RPGManager>
{
    public bool GameOver => !player.Alive;

    [SerializeField]
    private PromoCodeWindow promoCodeWindow = default;

    [SerializeField]
    private LevelUpWindow levelUpWindow = default;

    [SerializeField]
    private RPGEntity player = default;

    [SerializeField]
    private UnityEngine.UI.Text playerText = default;

    [SerializeField]
    private Transform playerSword = default;

    [SerializeField]
    private Sprite promoSword = default;

    [SerializeField]
    private BarUI expBar = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT1 = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT2 = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT3 = default;

    [SerializeField]
    private GameObject damageSplashPrefab = default;

    [SerializeField]
    private List<DialogueEvent> levelUpEvents = default;

    private int playerExp;
    private AudioSource rpgMusic;
    private bool usedPromoSword;

    private void Start()
    {
        rpgMusic = AudioController.Instance.PlaySound2D("rpg_track", 0.7f, 0f);
        rpgMusic.loop = true;
        BuddyHandsController.Instance.SetLeftHandTarget(playerSword);
        UpdateNameText();
        GameStatus.isRPG = true;
        StartCoroutine(GameLoop());
    }

    public void SwordPromoCode()
    {
        if (!usedPromoSword)
        {
            player.attackPower += 10;
            usedPromoSword = true;

            var swordSR = playerSword.GetComponent<SpriteRenderer>();
            swordSR.sprite = promoSword;
            CustomCoroutine.FlickerSequence(() => swordSR.enabled = true, () => swordSR.enabled = false, false, true, 0.07f, 4);
        }
    }

    private void UpdateNameText()
    {
        playerText.text = BuddyNameGenerator.GetName() + " ~ Lvl " + GameStatus.buddyLevel;
    }

    private IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(3f);

        while (player.Alive)
        {
            yield return new WaitForSeconds(0.6f);

            var enemy = InstantiateEnemy();
            yield return new WaitForSeconds(0.5f);

            yield return DoBattle(enemy);

            if (player.Alive)
            {
                yield return new WaitForSeconds(0.1f);

                playerExp += enemy.expReward;
                Destroy(enemy.gameObject);
                AudioController.Instance.PlaySound2D("enemy_die", 0.7f);
                ScreenEffectsController.Instance.AddThenSubtractIntensity(ScreenEffect.RenderCanvasShake, 0.05f, 0.05f, 0.05f, 0.1f);

                int expToNextLevel = Mathf.RoundToInt(Mathf.Pow(GameStatus.buddyLevel * 10f, 1.015f));
                expBar.ShowAmount(playerExp / (float)expToNextLevel);
                yield return new WaitForSeconds(0.5f);

                if (playerExp > expToNextLevel)
                {
                    yield return new WaitWhile(() => promoCodeWindow.Active);
                    yield return LevelUp();
                }
            }
            else
            {
                Exit();
            }
        }
    }

    private void Exit()
    {
        Destroy(rpgMusic.gameObject);
        BuddyHandsController.Instance.ClearHandTargets();
        gameObject.SetActive(false);
        GameStatus.isRPG = false;
    }

    private IEnumerator LevelUp()
    {
        playerExp = 0;
        GameStatus.buddyLevel++;
        expBar.ShowAmount(0f, true);

        levelUpWindow.ShowLevelUp();
        yield return new WaitWhile(() => levelUpWindow.Choosing);

        CustomCoroutine.FlickerSequence(() => playerText.enabled = true, () => playerText.enabled = false, false, true, 0.1f, 3);
        UpdateNameText();

        switch (GameStatus.buddyLevel)
        {
            case 2:
                DialogueHandler.Instance.AddDialogueEventToStack(levelUpEvents[0]);
                break;
            case 3:
                DialogueHandler.Instance.AddDialogueEventToStack(levelUpEvents[1]);
                break;
            case 4:
                DialogueHandler.Instance.AddDialogueEventToStack(levelUpEvents[2]);
                break;
            case 7:
                DialogueHandler.Instance.AddDialogueEventToStack(levelUpEvents[3]);
                break;
            case 10:
                DialogueHandler.Instance.AddDialogueEventToStack(levelUpEvents[4]);
                break;
        }
    }

    private RPGEnemy InstantiateEnemy()
    {
        var enemyList = enemyPrefabsT1;
        if (GameStatus.buddyLevel >= 4)
        {
            enemyList = enemyPrefabsT2;
        }
        if (GameStatus.buddyLevel >= 10)
        {
            enemyList = enemyPrefabsT3;
        }

        var obj = Instantiate(enemyList[Random.Range(0, enemyList.Count)]);

        obj.transform.parent = transform;
        Vector2 battlePos = obj.transform.position;
        obj.transform.position += Vector3.right * 2f;

        Tween.Position(obj.transform, battlePos, 0.5f, 0f, Tween.EaseOut);

        var enemy = obj.GetComponent<RPGEnemy>();
        return enemy;
    }
    
    private IEnumerator DoBattle(RPGEnemy enemy)
    {
        while (player.Alive && enemy.Alive)
        {
            yield return EntityAttackEntity(enemy, player);
            yield return new WaitForSeconds(0.4f);
            yield return new WaitWhile(() => promoCodeWindow.Active);
            if (player.Alive)
            {
                yield return EntityAttackEntity(player, enemy);
                yield return new WaitForSeconds(0.4f);
                yield return new WaitWhile(() => promoCodeWindow.Active);
            }
        }
    }

    private IEnumerator EntityAttackEntity(RPGEntity attacker, RPGEntity defender)
    {
        yield return attacker.PlayAttackAnim();
        defender.TakeHitAnim();

        bool crit = false;
        int damage = attacker.attackPower;
        if (Random.value < attacker.critChance)
        {
            damage *= attacker.critMultiplier;
            crit = true;
            AudioController.Instance.PlaySound2D("rpg_crit", 0.5f);
        }
        defender.health -= damage;
        defender.UpdateHealthBar();

        if (defender is RPGEnemy)
        {
            SpawnDamageSplash(damage, crit, defender.transform.position + Vector3.up * 0.3f);
        }
        else
        {
            ScreenEffectsController.Instance.AddThenSubtractIntensity(ScreenEffect.RenderCanvasShake, 0.05f, 0.05f, 0.05f, 0.1f);
            SpawnDamageSplash(damage, crit, Vector3.up * 0.2f);
        }
    }

    private void SpawnDamageSplash(int damage, bool crit, Vector2 pos)
    {
        var obj = Instantiate(damageSplashPrefab);
        obj.transform.position = pos;
        obj.GetComponent<DamageSplash>().UpdateDisplay(damage, crit);
        Destroy(obj, 0.5f);
    }
}
