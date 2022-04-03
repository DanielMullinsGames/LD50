using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class RPGManager : Singleton<RPGManager>
{
    [SerializeField]
    private RPGEntity player = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT1 = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT2 = default;

    [SerializeField]
    private List<GameObject> enemyPrefabsT3 = default;

    private void Start()
    {
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
                // IF PLAYER WINS, EXP + LEVEL UP
                Destroy(enemy.gameObject);
            }
            else
            {
                // ELSE EXIT RPG
            }
        }
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
