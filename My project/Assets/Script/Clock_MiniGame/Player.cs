using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    GameManager gameManager;
    NetworkTransform networkTransform;
    bool isTeleport = false;
    GUIStyle guiStyle;

    void Awake()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 72;

    }

    void Update()
    {
        if(gameManager == null) gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(networkTransform == null) networkTransform = GetComponent<NetworkTransform>();
    }

    public override void FixedUpdateNetwork()
    {
        // Teleport();
    }

    void Teleport()
    {

        if(isTeleport) return;
        if(gameManager.playerCount <= 0) networkTransform.Teleport(gameManager.networkPlayers[0].transform.position);   
        else networkTransform.Teleport(gameManager.networkPlayers[gameManager.playerCount].transform.position);
        isTeleport = true;
    }

    void OnGUI()
    {
        NetworkObject networkObject = new NetworkObject();
        GUI.Label(new Rect(10, 10 + gameManager.playerCount * 50, 300, 20), networkObject.InputAuthority.ToString(), guiStyle);
    }
}
