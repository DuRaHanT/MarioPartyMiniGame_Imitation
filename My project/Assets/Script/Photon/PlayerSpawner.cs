using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public Transform[] spawnPoints; // 스폰 포인트 배열
    public GameObject playerPrefab;

    public int playerIndex = 0;


    public void PlayerJoined(PlayerRef player)
    {
        // 플레이어 참가 로그 출력
        Debug.Log($"Player {player.PlayerId} Joined aaaaaaa");

        playerIndex++;

        //if(player == Runner.LocalPlayer)
        //{
        //    Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        //}

        // 플레이어 스폰
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} Level aaaaaaa");

        playerIndex++;
    }

    void SpawnPlayer(PlayerRef player)
    {
        // 스폰 포인트 배열에서 위치 선택
        //int Index = player.PlayerId % spawnPoints.Length;
        Transform spawnPoint = spawnPoints[playerIndex]; // 지금은 int값을 받아오지만 플레이어가 나갔다가 들어오면 이전 플레이어도 기록이 되어 max값을 넘겨 버림

        // 플레이어를 해당 위치에 스폰
        // 여기서 `playerPrefab`은 스폰할 플레이어의 프리팹

        if(player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPrefab, spawnPoint.position, spawnPoint.rotation, player);
        }
    }
}