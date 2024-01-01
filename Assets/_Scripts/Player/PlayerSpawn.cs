using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform spawnPoint; // Место спауна

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // Создаем или находим объект персонажа
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Если объект персонажа не найден, выводим предупреждение
        if (player == null)
        {
            Debug.LogWarning("Player object not found. Make sure the player has the 'Player' tag.");
            return;
        }

        // Помещаем персонажа на место спауна
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
    }
}