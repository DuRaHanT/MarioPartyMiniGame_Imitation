using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Transform[] CenterPosition;
    GameObject[] chronoClashPosition;

    void Awake()
    {
        Center_Script[] center = FindObjectsOfType<Center_Script>();

        for(int i = 0;i < center.Length;i++)
        {
            CenterPosition[i] = center[i].transform;
        }

        ChronoClashManager[] chronoClash = FindObjectsOfType<ChronoClashManager>();

        for(int i = 0; i < chronoClash.Length; i++)
        {
            chronoClashPosition[i] = chronoClash[i].gameObject; 
        }
    }

    void UpBarRotation()
    {

    }

    void DownBarRotation()
    {

    }
}