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

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            Generate();
        }
        GenerateNavmesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateNavmesh()
    {
        NavMeshSurface surface = gameObject.AddComponent<NavMeshSurface>();
        surface.layerMask = navigatableLayers;
        surface.collectObjects = CollectObjects.Children;
        surface.BuildNavMesh();
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
    }
}
