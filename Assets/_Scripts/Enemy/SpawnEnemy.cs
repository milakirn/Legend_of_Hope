using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}