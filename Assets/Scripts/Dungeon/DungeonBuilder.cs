using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonBuilder : MonoBehaviour
{
    public HeroUnit playerHeroPrefab;
    public DungeonSegment[] segmentPrefabs;
    public Transform[] interactablePrefabs;
    public Light spotlightPrefab;
    public LayerMask navigatableLayers;

    HeroUnit playerHero;
    List<DungeonSegment> segments = new List<DungeonSegment>();
    NavMeshSurface navSurface;

    // Start is called before the first frame update
    void Start()
    {
        navSurface = gameObject.AddComponent<NavMeshSurface>();
        navSurface.layerMask = navigatableLayers;
        navSurface.collectObjects = CollectObjects.Children;

        Generate(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnHeroes()
    {
        playerHero = Instantiate(playerHeroPrefab);
        playerHero.GetComponent<UnitMovementController>().Disable();
        playerHero.transform.position = transform.position;
        Vector3 spawnDir = -Vector3.right;
        foreach (HeroUnit hero in PartyManager.it.party)
        {
            hero.transform.position = playerHero.transform.position + spawnDir * 2.0f;
            hero.GetMovementController().SetLeader(playerHero);
            hero.ClearUnitsInRange();
            hero.GetMovementController().enabled = true;
            hero.GetAbilityController().enabled = true;
            spawnDir = Quaternion.AngleAxis(-90, Vector3.up) * spawnDir;
        }
    }

    public IEnumerator GenerateNavmeshAndComplete(DungeonSegment segment, bool first = false)
    {
        navSurface.BuildNavMesh();
        yield return new WaitForSeconds(1.0f);
        segment.CompleteGenerate();
        if (first)
        {
            playerHero.GetMovementController().Enable();
            foreach (HeroUnit hero in PartyManager.it.party)
            {
                hero.GetMovementController().Enable();
            }
        }
    }

    public void Generate(bool first = false)
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
        segments.Add(segment);

        if (first) SpawnHeroes();

        segment.StartGenerate(segments.Count > 1);
        StartCoroutine(GenerateNavmeshAndComplete(segment, first));
    }
}
