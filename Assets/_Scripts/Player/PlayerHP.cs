using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private float playerHp = 100f;
    private float testDamage = 20f;

    public float PlayerHp
    { get { return playerHp; } }

    [SerializeField]
    private Slider playerHPSlider;

    private void Start()
    {
        playerHp = 100f;
        playerHPSlider.value = playerHp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeHit(testDamage);
            Debug.Log($"Taken damage: {testDamage}, current HP is {playerHp}");
        }
    }

    public void TakeHit(float damage)
    {
        playerHp -= damage;
        playerHPSlider.value = playerHp;
    }
}