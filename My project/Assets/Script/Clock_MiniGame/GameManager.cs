using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    Transform[] centerPosition;
    public TextMeshProUGUI scoreText;

    int upBar_index = 0;
    int downBar_index = 0;
    [HideInInspector] public int score = 0;
    [HideInInspector] public int currentScore = 0;
    int scoreThreshold = 10;
    float rotationSpeed = 50.0f;  // minimum = 50.0f  maximum = 350.0f
    float speedChangeThreshold = 30.0f;

    void Awake()
    {
        Center_Script[] center = FindObjectsOfType<Center_Script>();

        centerPosition = new Transform[center.Length];

        for (int i = 0;i < center.Length;i++)
        {
            centerPosition[i] = center[i].transform;
        }
    }

    void Start() => CenterIndexCheck();

    private void Update()
    {
        RotationCenter(upBar_index);

        if(currentScore >= scoreThreshold)
        {
            SetSpeed();
            currentScore = 0;
        }
    }

    void CenterIndexCheck()
    {
        for(int i = 0; i < centerPosition.Length; i++)
        {
            if (centerPosition[i].name == "Up_Center") upBar_index = i;
            if (centerPosition[i].name == "Down_Center") downBar_index = i;
        }
    }

    void RotationCenter(int index)
    {
        centerPosition[index].Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    public void Score()
    {
        scoreText.text = $"Score: {score.ToString()}";
    }

    void SetSpeed()
    {
        float randomValue = Random.value;

        if (randomValue < 0.9f)
        {
            rotationSpeed = Random.Range(50.0f, 350.0f);
            Debug.Log($"90% {rotationSpeed}");
        }

        else
        {
            if(Mathf.Abs(centerPosition[upBar_index].rotation.eulerAngles.y / centerPosition[downBar_index].rotation.eulerAngles.y) <= speedChangeThreshold)
            {
                rotationSpeed = 300.0f;
            }
            else
            {
                rotationSpeed = 50.0f;
            }
            Debug.Log($"10% {rotationSpeed}");
        }
    }
}