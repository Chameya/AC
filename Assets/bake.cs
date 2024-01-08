using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bake : MonoBehaviour
{
    NavMeshSurface navMeshSurface;
    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        navMeshSurface.BuildNavMesh();
    }
}
