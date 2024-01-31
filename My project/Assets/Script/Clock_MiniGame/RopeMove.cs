using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RopeMove : NetworkBehaviour
{
    private float xMin; //-8.61f
    private float xMax; //11.3f
    [Header("이동 속도 조절값")]
    public float _sinMultiplier = 5f; // 이동속도 조절값
    [Header("줄 높이 조절값")]
    public float _cosMultiplier = 5f; // 줄 높이 조절값

    private float timeValue = 0f;

    private void Start()
    {
        GameObject mapObject = GameObject.FindWithTag("Map");

        if (mapObject == null) return;

        Bounds mapBounds = mapObject.GetComponent<Renderer>().bounds;
        xMin = mapBounds.min.x;
        xMax = mapBounds.max.x;

    }

    public override void FixedUpdateNetwork()
    {
        float xValue = Mathf.Sin(timeValue * _sinMultiplier);
        float yValue = Mathf.Cos(timeValue * _sinMultiplier);

        float xPos = xValue * (xMax - xMin) * 0.5f;
        float yPos = yValue * _cosMultiplier;

        yPos = Mathf.Clamp(yPos, 0.1f, 10f);

        transform.position = new Vector3(xPos, yPos, 0.0f);

        timeValue += Runner.DeltaTime;
    }

}