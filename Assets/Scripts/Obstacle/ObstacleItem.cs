using System;
using UnityEngine;

public class ObstacleItem : MonoBehaviour
{
    [Header("Data Reference")]
    [SerializeField] private ObstacleData obstacleData;
    
    public ObstacleData GetData() => obstacleData;

    private void OnEnable()
    {
        PlayerCollision.Register(this);
    }

    private void OnDisable()
    {
        PlayerCollision.Deregister(this);
    }
}
