using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    private float enemyHP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyHP = other.GetComponent<EnemyHP>().EnemyHealth;
            if (playerAnimator.GetBool("LightAttack"))
            {
                DealDamage(5);
                other.GetComponent<EnemyHP>().EnemyHealth = enemyHP;
            }
            else if (playerAnimator.GetBool("HeavyAttack"))
            {
                DealDamage(15);
                other.GetComponent<EnemyHP>().EnemyHealth = enemyHP;
            }
        }
    }

    private void DealDamage(int damage)
    {
        enemyHP -= damage;

        Debug.Log($"Dealt {damage} damage to enemy");
    }
}