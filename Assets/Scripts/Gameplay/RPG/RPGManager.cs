using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGManager : Singleton<RPGManager>
{
    [SerializeField]
    private RPGEntity player = default;

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (player.Alive)
        {
            // INSTANTIATE ENEMY

            // BATTLE

            // IF PLAYER WINS, EXP + LEVEL UP
        }
        yield break;
    }
    
    private IEnumerator DoBattle(RPGEnemy enemy)
    {
        while (player.Alive && enemy.Alive)
        {
            yield return EntityAttackEntity(enemy, player);

            if (player.Alive)
            {
                yield return EntityAttackEntity(player, enemy);
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
