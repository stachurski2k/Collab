using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmeshBaker : MonoBehaviour
{
    public NavMeshSurface surface;
    public bool bakeOnStart = false;

    private void Start()
    {
        if (bakeOnStart)
        {
            bakeNavMesh();
        }
    }

    public void bakeNavMesh()
    {
        surface.BuildNavMesh();
    }
}
