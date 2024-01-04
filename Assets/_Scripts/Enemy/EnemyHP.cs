using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    private float enemyHealth = 20f;

    public float EnemyHealth
    { get { return enemyHealth; } set { enemyHealth = value; } }

    private void Update()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}