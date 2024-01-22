using Fusion;
using UnityEngine;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject spawnedPlayer = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);

            SetParent(spawnedPlayer);

            ResetTransform(spawnedPlayer);
        }
    }

    void SetParent(NetworkObject player)
    {
        Transform parentTransform = transform;

        player.transform.parent = parentTransform;
    }

    void ResetTransform(NetworkObject player)
    {
        Vector3 spawnPosition = new Vector3(0,1,0);
        Quaternion spawnRotation = Quaternion.identity;
        Vector3 spawnScale = new Vector3(0.1f, 1.0f, 1.0f);

        player.transform.position = spawnPosition;
        player.transform.rotation = spawnRotation;
        player.transform.localScale = spawnScale;
    }
}
