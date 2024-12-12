using System.Collections.Generic;
using NPC;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] LayerMask mask;
    [SerializeField, Range(0, 10)] private int spawnBoidAmount;
    [SerializeField] private NpcStats playerStats;
    [SerializeField] private NpcStats boidStats;
    [SerializeField] GameObject bluePrefab;
    [SerializeField] GameObject redPrefab;
    [SerializeField] Transform blueFlag;
    [SerializeField] Transform redFlag;
    private PlayerNpc blueLider;
    private PlayerNpc redLider;
    private Npc[] _allBoids;
    public Npc[] GetAllBoids() => _allBoids;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        InitializeNpcs();
    }

    private void InitializeNpcs()
    {
        var boids = new List<Npc>((spawnBoidAmount * 2)+2);
        blueLider = Instantiate(bluePrefab).AddComponent<PlayerNpc>();
        redLider = Instantiate(redPrefab).AddComponent<PlayerNpc>();
        blueLider.Initialize(ColorBand.Blue).SetStats(playerStats).SetTarget(blueFlag);
        redLider.Initialize(ColorBand.Red).SetStats(playerStats).SetTarget(redFlag);
        boids.Add(blueLider);
        boids.Add(redLider);
        for (int i = 0; i < spawnBoidAmount; i++)
        {
            var blue = Instantiate(bluePrefab).AddComponent<BoidNpc>();
            var red = Instantiate(redPrefab).AddComponent<BoidNpc>();
            blue.Initialize(ColorBand.Blue).SetStats(boidStats).SetTarget(blueLider.transform);
            red.Initialize(ColorBand.Red).SetStats(boidStats).SetTarget(redLider.transform);
            boids.Add(blueLider);
            boids.Add(redLider);
        }

        _allBoids = boids.ToArray();
    }

    private void OnValidate()
    {
        if(blueLider)blueLider.SetStats(boidStats);
        if(redLider)redLider.SetStats(boidStats);
    }

    public bool LosToWall(Vector3 start, Vector3 end)
    {
        return LineOfSight.LoSDetectMask(start, end, mask);
    }

    //public Player GetLider(ColorBand _color)
    //{
    //    if (_color.Equals(ColorBand.Red))
    //    {
    //        return _redPlayer;
    //    }
    //    else
    //    {
    //        return _bluePlayer;
    //    }
    //}
}