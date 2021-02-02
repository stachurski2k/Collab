using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGeneration;
namespace MapGeneration{
public class GPS : MonoBehaviour
{
    Generator currentMap;
    void Start()
    {
        Map.instance.OnMapGenerated+=HandleMapGenerated;
    }
    void HandleMapGenerated(){
        currentMap=Map.instance.generators[0];
        print(currentMap.entry);
        print(currentMap.exit);
        PathFinder.BeginBuild(currentMap,currentMap.entry,currentMap.exit,HandlePathReceived);
    }
    int HandlePathReceived(List<PathMarker> path){
        foreach (var pos in path)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position=new Vector3(pos.location.x,0f,pos.location.z)*currentMap.scale;
            go.GetComponent<Renderer>().material.color=Color.blue;
        }
        return 0;
    }
}
}