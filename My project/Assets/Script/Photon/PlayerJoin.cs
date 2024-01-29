using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerJoin : NetworkBehaviour, IPlayerJoined
{
    public GameObject playerPrefab;

    public virtual void OnPlayerJoined(PlayerRef player)
    {
        Debug.Log("Player Joined");
        PlayerJoined(player);
    }

    public void PlayerJoined(PlayerRef player)
    {
        Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
    }
}
