using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [SerializeField] private List<Enemy> enemyList;
    public Action<string> detectTarget;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (Enemy enemy in enemyList)
        {
            detectTarget += enemy.ChangeState;
        }
    }
}
