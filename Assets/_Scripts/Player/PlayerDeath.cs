using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private PlayerHP PlayerHP;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject deathPanel;

    private bool isCoroutineStart;

    private void Start()
    {
        PlayerHP = FindObjectOfType<PlayerHP>();
    }

    private void Update()
    {
        if (PlayerHP.PlayerHp <= 0)
        {
            deathPanel.SetActive(true);
            player.SetActive(false);
            if (!isCoroutineStart)
            {
                StartCoroutine(Death(3f));
            }
        }
    }

    private IEnumerator Death(float waitBeforeRestart)
    {
        isCoroutineStart = true;
        Debug.Log("Start coroutine");
        Debug.Log("Start timer" + Time.time);

        yield return new WaitForSeconds(waitBeforeRestart);

        Debug.Log("Start timer" + Time.time);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Stop Coroutine");
        StopAllCoroutines();
    }
}