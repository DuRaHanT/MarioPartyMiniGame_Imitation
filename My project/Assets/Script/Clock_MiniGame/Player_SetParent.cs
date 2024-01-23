using Fusion;
using UnityEngine;

public class Player_SetParent : NetworkBehaviour
{
    public GameObject parent;
    public GameObject player;

    public void SetParent()
    {
        Transform parentTransform = parent.transform;

        player.transform.parent = parentTransform;

        player.GetComponent<NetworkTransform>().Teleport(Vector3.zero, Quaternion.identity);
    }

    public void ResetTransfom(int index)
    {
        Vector3 spawnPosition = new Vector3(0.0f, 0.0f, -1.25f - (index * 0.75f));
        Vector3 spawnScale = new Vector3(0.05f, 0.1f, 1.0f);

        player.transform.localScale = spawnScale;

        player.GetComponent<NetworkTransform>().Teleport(spawnPosition);

    }
}
