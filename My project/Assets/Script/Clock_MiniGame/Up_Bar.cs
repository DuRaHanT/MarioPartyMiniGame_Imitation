using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Up_Bar : MonoBehaviour
{
    GameManager gameManager;
    bool isTrigger = false;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {
        //InvokeRepeating("ScoreUp", 0f, 1f);    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isTrigger = true;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTrigger = false;
    }

    void ScoreUp()
    {
        if(isTrigger == false)
        {
            gameManager.score++;
            gameManager.currentScore++;
        }

        gameManager.Score();
    }
}
