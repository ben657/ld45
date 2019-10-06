using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonBuilder : MonoBehaviour
{
    public DungeonSegment[] segmentPrefabs;
    public Transform[] interactablePrefabs;
    public Light spotlightPrefab;
    public LayerMask navigatableLayers;

    List<DungeonSegment> segments = new List<DungeonSegment>();
    NavMeshSurface navSurface;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHeroController playerController = FindObjectOfType<PlayerHeroController>();
        Unit playerHero = playerController.unit;

        navSurface = gameObject.AddComponent<NavMeshSurface>();
        navSurface.layerMask = navigatableLayers;
        navSurface.collectObjects = CollectObjects.Children;

        Generate();

        Vector3 spawnDir = -Vector3.right;
        foreach(HeroUnit hero in PartyManager.it.party)
        {
            hero.transform.position = spawnDir * 2.0f;
            hero.GetMovementController().SetLeader(playerHero);
            hero.ClearUnitsInRange();
            hero.GetMovementController().enabled = true;
            hero.GetAbilityController().enabled = true;
            spawnDir = Quaternion.AngleAxis(90, Vector3.up) * spawnDir;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateNavmesh()
    {
        navSurface.BuildNavMesh();
    }

    public void Generate()
    {
        DungeonSegment prefab = segmentPrefabs[Random.Range(0, segmentPrefabs.Length)];
        DungeonSegment segment = Instantiate(prefab);
        segment.builder = this;

        Vector3 position = transform.position;
        if(segments.Count > 0)
        {
            position = segments[segments.Count - 1].GetExitPosition();
        }
        segment.transform.parent = transform;
        segment.transform.position = position;

        segment.Generate();

        segments.Add(segment);
        GenerateNavmesh();
    }
}
