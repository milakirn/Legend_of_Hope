using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform spawnPoint; // ����� ������

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // ������� ��� ������� ������ ���������
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // ���� ������ ��������� �� ������, ������� ��������������
        if (player == null)
        {
            Debug.LogWarning("Player object not found. Make sure the player has the 'Player' tag.");
            return;
        }

        // �������� ��������� �� ����� ������
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
    }
}