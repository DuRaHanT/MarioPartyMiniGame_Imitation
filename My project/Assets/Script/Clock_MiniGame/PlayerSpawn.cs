using System.Collections;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public GameManager gameManager;

    void Awake() 
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();    
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {   
           Runner.Spawn(PlayerPrefab, Vector3.zero, Quaternion.identity, player); 
        }
    }
}
