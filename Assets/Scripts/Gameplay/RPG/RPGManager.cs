﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class RPGManager : Singleton<RPGManager>
{
    public bool GameOver => !player.Alive;

    [SerializeField]
    private LevelUpWindow levelUpWindow = default;

    [SerializeField]
    private RPGEntity player = default;

    [SerializeField]
    private UnityEngine.UI.Text playerText = default;

    [SerializeField]
    private Transform playerSword = default;

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

    private int playerExp;
    private AudioSource rpgMusic;

    private void Start()
    {
        rpgMusic = AudioController.Instance.PlaySound2D("rpg_track", 0.7f, 0f);
        rpgMusic.loop = true;
        BuddyHandsController.Instance.SetLeftHandTarget(playerSword);
        UpdateNameText();
        StartCoroutine(GameLoop());
    }

    private void UpdateNameText()
    {
        playerText.text = BuddyNameGenerator.GetName() + " ~ Lvl " + GameStatus.buddyLevel;
    }

    private IEnumerator GameLoop()
    {
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

                int expToNextLevel = Mathf.RoundToInt(Mathf.Pow(GameStatus.buddyLevel * 10f, 1.05f));
                expBar.ShowAmount(playerExp / (float)expToNextLevel);
                yield return new WaitForSeconds(0.5f);

                if (playerExp > expToNextLevel)
                {
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
    }

    private RPGEnemy InstantiateEnemy()
    {
        var enemyList = enemyPrefabsT1;
        if (GameStatus.buddyLevel > 5)
        {
            enemyList = enemyPrefabsT2;
        }
        if (GameStatus.buddyLevel > 15)
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

            if (player.Alive)
            {
                yield return EntityAttackEntity(player, enemy);
                yield return new WaitForSeconds(0.4f);
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
