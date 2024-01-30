using Fusion;
using UnityEngine;
using TMPro;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public Transform[] spawnPoints;
    public GameObject playerPrefab;
    [Networked] public int NetworkedPlayerIndex { get; set; } = -1;
    public TextMeshProUGUI text;

    public override void FixedUpdateNetwork() 
    {
        SetText();
    }

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} Joined aaaaaaa");
        NetworkedPlayerIndex++;
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} Level aaaaaaa");
        NetworkedPlayerIndex--;
    }

    void SpawnPlayer(PlayerRef player)
    {
        Transform spawnPoint = spawnPoints[NetworkedPlayerIndex];

        if(player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPrefab, spawnPoint.position, Quaternion.identity, player);
        }
    }

    public void SetText()
    {
        text.text = NetworkedPlayerIndex.ToString();
    }
}