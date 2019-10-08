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

    public DungeonCameraController cameraController;
    public float segmentFallHeight = 5.0f;
    public float segmentFallTime = 0.0f;
    public AnimationCurve segmentFallCurve;

    HeroUnit playerHero;
    List<DungeonSegment> segments = new List<DungeonSegment>();

    // Start is called before the first frame update
    public void Build()
    {
        Generate(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHero && playerHero.IsDead())
        {
            PartyManager.it.LoadInn();
        }
    }

    void SpawnHeroes(DungeonSegment segment)
    {
        playerHero = Instantiate(playerHeroPrefab);
        playerHero.transform.parent = segment.transform;
        cameraController.target = playerHero.transform;
        playerHero.GetComponent<UnitMovementController>().Disable();
        playerHero.transform.position = segment.transform.position;
        Vector3 spawnDir = -Vector3.right;
        foreach (HeroUnit hero in PartyManager.it.party)
        {
            hero.transform.position = playerHero.transform.position + spawnDir * 2.0f;
            hero.transform.parent = segment.transform;
            hero.GetMovementController().SetLeader(playerHero);
            hero.ClearUnitsInRange();
            hero.GetMovementController().Enable();
            hero.GetAbilityController().enabled = true;
            spawnDir = Quaternion.AngleAxis(-90, Vector3.up) * spawnDir;
        }
    }

    public IEnumerator GenerateNavmeshAndComplete(DungeonSegment segment, bool first = false)
    {
        segment.GenerateNavMesh();
        float s = 0.0f;
        Vector3 pos;
        while(s <= 1.0f)
        {
            yield return new WaitForFixedUpdate();
            pos = segment.transform.position;
            pos.y = segmentFallHeight - segmentFallCurve.Evaluate(Mathf.Clamp01(s)) * segmentFallHeight;
            segment.transform.position = pos;
            s += Time.fixedDeltaTime / segmentFallTime;
        }
        if (first) SpawnHeroes(segment);
        segment.CompleteGenerate(!first);
        if (first)
        {
            playerHero.GetComponent<UnitMovementController>().Enable();
            foreach (HeroUnit hero in PartyManager.it.party)
            {
                hero.GetComponent<UnitMovementController>().Enable();
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
        segment.transform.position = position + Vector3.up * segmentFallHeight;
        segments.Add(segment);

        segment.StartGenerate();
        StartCoroutine(GenerateNavmeshAndComplete(segment, first));
    }
}
