using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody player;

    float jumpPower = 5.0f;

    bool isBar = true;

    void Awake()
    {
        player = GetComponent<Rigidbody>();
    }

    public void Jump()
    {
        if (isBar)
        {
            player.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Down_Bar") isBar = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isBar = false;
    }
}
