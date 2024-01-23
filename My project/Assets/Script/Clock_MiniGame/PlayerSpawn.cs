using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public GameObject parent;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject playerObject = Runner.Spawn(PlayerPrefab, Vector3.down * 10, Quaternion.identity, player);

            // SetParent(playerObject);
            // SetPosition(playerObject);
        }
    }

    void SetParent(NetworkObject player)
    {
        Transform parentTransform = parent.transform;

        player.transform.parent = parentTransform;

        player.GetComponent<NetworkTransform>().Teleport(Vector3.zero, Quaternion.identity);
    }

    void SetPosition(NetworkObject player)
    {
        GameObject[] playerCount = GameObject.FindGameObjectsWithTag("Player");

        Vector3 spawnPosition = new Vector3(0.0f, 0.0f, -1.25f - (playerCount.Length * 0.75f));
        Vector3 spawnScale = new Vector3(0.05f, 0.1f, 1.0f);

        player.transform.localScale = spawnScale;

        player.GetComponent<NetworkTransform>().Teleport(spawnPosition);

        Debug.Log("Teleport");
    }
}
