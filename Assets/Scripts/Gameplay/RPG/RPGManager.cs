using System.Collections;
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
    private BarUI expBar = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT1 = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT2 = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT3 = default;

    private int playerExp;
    private AudioSource rpgMusic;

    private void Start()
    {
        rpgMusic = AudioController.Instance.PlaySound2D("rpg_track", 0.7f, 0f);
        rpgMusic.loop = true;
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (player.Alive)
        {
            yield return new WaitForSeconds(1f);

            var enemy = InstantiateEnemy();
            yield return new WaitForSeconds(0.5f);

            yield return DoBattle(enemy);

            if (player.Alive)
            {
                playerExp += enemy.expReward;
                Destroy(enemy.gameObject);
                
                int expToNextLevel = Mathf.RoundToInt(Mathf.Pow(GameStatus.buddyLevel * 10f, 1.1f));
                expBar.ShowAmount(playerExp / (float)expToNextLevel);
                yield return new WaitForSeconds(0.5f);

                if (playerExp > expToNextLevel)
                {
                    yield return LevelUp();
                }
            }
            else
            {
                // ELSE EXIT RPG
            }
        }
    }

    private IEnumerator LevelUp()
    {
        playerExp = 0;
        GameStatus.buddyLevel++;
        expBar.ShowAmount(0f, true);

        levelUpWindow.ShowLevelUp();
        yield return new WaitWhile(() => levelUpWindow.Choosing);
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

        int damage = attacker.attackPower;
        if (Random.value < attacker.critChance)
        {
            damage *= attacker.critMultiplier;
            //SHOW CRIT
        }
        defender.health -= damage;
        defender.UpdateHealthBar();

        //SHOW DAMAGE NUMBERS
    }
}
