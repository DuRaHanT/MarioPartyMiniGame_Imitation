using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public Transform[] spawnPoints; // ���� ����Ʈ �迭
    public GameObject playerPrefab;

    public int playerIndex = 0;


    public void PlayerJoined(PlayerRef player)
    {
        // �÷��̾� ���� �α� ���
        Debug.Log($"Player {player.PlayerId} Joined aaaaaaa");

        playerIndex++;

        //if(player == Runner.LocalPlayer)
        //{
        //    Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        //}

        // �÷��̾� ����
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} Level aaaaaaa");

        playerIndex++;
    }

    void SpawnPlayer(PlayerRef player)
    {
        // ���� ����Ʈ �迭���� ��ġ ����
        //int Index = player.PlayerId % spawnPoints.Length;
        Transform spawnPoint = spawnPoints[playerIndex]; // ������ int���� �޾ƿ����� �÷��̾ �����ٰ� ������ ���� �÷��̾ ����� �Ǿ� max���� �Ѱ� ����

        // �÷��̾ �ش� ��ġ�� ����
        // ���⼭ `playerPrefab`�� ������ �÷��̾��� ������

        if(player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPrefab, spawnPoint.position, spawnPoint.rotation, player);
        }
    }
}