﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonSegment : MonoBehaviour
{
    public Transform[] interactablePositions;
    public Transform[] lightPositions;
    public Transform[] monsterPositions;
    public MonsterUnit[] monsterPrefabs;

    public int minInteractables = 1;
    public int maxInteractables = 5;
    public float interactableChance = 0.5f;
    public float lightChance = 0.5f;
    public float monsterChance = 0.5f;

    public DungeonBuilder builder { get; set; }

    BoxCollider boundsCollider;
    bool triggered = false;

    List<MonsterUnit> spawnedMonsters = new List<MonsterUnit>();
    
    void Awake()
    {
        boundsCollider = GetComponent<BoxCollider>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Bounds GetBounds()
    {
        return boundsCollider.bounds;
    }

    void SpawnInteractable(Transform t)
    {
        Transform prefab = builder.interactablePrefabs[Random.Range(0, builder.interactablePrefabs.Length)];
        Transform interactable = Instantiate(prefab);
        interactable.parent = t;
        interactable.position = t.position;
    }

    public void StartGenerate(bool monsters = true)
    {
        int interactablesSpawned = 0;
        List<Transform> possiblePositions = new List<Transform>(interactablePositions);
        for(int i = 0; i < minInteractables; i++)
        {
            if (interactablesSpawned == maxInteractables) break;
            int index = Random.Range(0, possiblePositions.Count);
            Transform t = possiblePositions[index];
            SpawnInteractable(t);

            interactablesSpawned += 1;
            possiblePositions.RemoveAt(index);
        }

        foreach(Transform t in interactablePositions)
        {
            if (interactablesSpawned == maxInteractables) break;
            if (Random.Range(0.0f, 1.0f) > interactableChance) continue;
            SpawnInteractable(t);

            interactablesSpawned += 1;   
        }

        foreach(Transform t in lightPositions)
        {
            t.gameObject.SetActive(Random.Range(0.0f, 1.0f) < lightChance);
        }
        
        if(monsters)
        {
            foreach (Transform t in monsterPositions)
            {
                if (Random.Range(0, 1.0f) > monsterChance) continue;
                MonsterUnit prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
                MonsterUnit unit = Instantiate(prefab, t.position, t.rotation);
                spawnedMonsters.Add(unit);
            }
        }
    }

    public void CompleteGenerate()
    {
        spawnedMonsters.ForEach(m => m.GetMovementController().GetAgent().enabled = true);
    }

    public Vector3 GetExitPosition()
    {
        Bounds bounds = GetBounds();
        Vector3 end = transform.position + Vector3.forward * bounds.size.z;
        float maxOffset = (bounds.size.x * 0.5f) - 2.0f;
        end += Vector3.right * Random.Range(-maxOffset, maxOffset);
        return end;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || other.isTrigger || !other.GetComponent<HeroUnit>()) return;
        builder.Generate();
        triggered = true;
    }
}
