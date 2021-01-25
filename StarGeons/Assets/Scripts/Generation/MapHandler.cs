using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] Map[] generators;
    [SerializeField] int mapSize;
    [SerializeField] GameObject stairs;
    [SerializeField] float levelOffset;
    [SerializeField] int scale=6;
    int level=0;
    List<MapLocation> bottomEnds=new List<MapLocation>();
    List<MapLocation> topEnds=new List<MapLocation>();
    private void Start()
    {
        foreach(var gen in generators){
            gen.levelOffset=levelOffset;
            gen.scale=scale;
            gen.Build(mapSize,level);
            level++;
        }
        for (int i = 0; i < generators.Length-1; i++)
        {
            if(PlaceStairs(i,180,Map.PieceType.DeadToRight,Map.PieceType.DeadToLeft))continue;
            if(PlaceStairs(i,-90,Map.PieceType.DeadUpsideDown,Map.PieceType.DeadEnd))continue;
             if(PlaceStairs(i,90,Map.PieceType.DeadEnd,Map.PieceType.DeadUpsideDown))continue;
            if(PlaceStairs(i,0,Map.PieceType.DeadToLeft,Map.PieceType.DeadToRight))continue;

        }
        foreach(var gen in generators){
            gen.MoveToOffsetPos();
        }
        
    }
    bool PlaceStairs(int i, float rotAngle,Map.PieceType lower,Map.PieceType upper){
        for (int x = 0; x < generators[i].width; x++)
        {
            for (int z = 0; z < generators[i].depth; z++)
            {
                if(generators[i].piecePlaces[x,z].piece==lower){
                    bottomEnds.Add(new MapLocation(x,z));
                }
                if(generators[i+1].piecePlaces[x,z].piece==upper){
                    topEnds.Add(new MapLocation(x,z));
                }
            }
        }
        if(bottomEnds.Count==0||topEnds.Count==0)return false;
        MapLocation stairsBottom=bottomEnds[Random.Range(0,bottomEnds.Count-1)];
        MapLocation stairsTop=topEnds[Random.Range(0,topEnds.Count-1)];
        Vector3 posBottom=stairsBottom.ToVector3(scale,generators[i].height);
        Vector3 posTop=stairsTop.ToVector3(scale,generators[i+1].height);

        //Remove old room and add stairs
        Destroy(generators[i].piecePlaces[stairsBottom.x,stairsBottom.z].model);
        Destroy(generators[i+1].piecePlaces[stairsTop.x,stairsTop.z].model);

        var stairsGO=Instantiate(stairs,posBottom,Quaternion.identity);
        stairsGO.transform.Rotate(0,rotAngle,0);
        stairsGO.transform.SetParent(generators[i].transform);

        generators[i].piecePlaces[stairsBottom.x,stairsBottom.z].model=stairsGO;
        generators[i+1].piecePlaces[stairsTop.x,stairsTop.z].model=stairsGO;

        generators[i].piecePlaces[stairsBottom.x,stairsBottom.z].piece=Map.PieceType.Lader;
        generators[i+1].piecePlaces[stairsTop.x,stairsTop.z].piece=Map.PieceType.Lader;
        //Calculate offset
        generators[i+1].xOffset=stairsBottom.x-stairsTop.x+generators[i].xOffset;
        generators[i+1].zOffset=stairsBottom.z-stairsTop.z+generators[i].zOffset;

        
        bottomEnds.Clear();
        topEnds.Clear();
        return true;
}
}

// if((generators[i].piecePlaces[x,z].piece==Map.PieceType.DeadEnd||
// generators[i].piecePlaces[x,z].piece==Map.PieceType.DeadToLeft||
// generators[i].piecePlaces[x,z].piece==Map.PieceType.DeadToRight||
// generators[i].piecePlaces[x,z].piece==Map.PieceType.DeadUpsideDown)
// &&
// (generators[i+1].piecePlaces[x,z].piece==Map.PieceType.DeadEnd||
// generators[i+1].piecePlaces[x,z].piece==Map.PieceType.DeadToLeft||
// generators[i+1].piecePlaces[x,z].piece==Map.PieceType.DeadToRight||
// generators[i+1].piecePlaces[x,z].piece==Map.PieceType.DeadUpsideDown)){
//     Vector3 pos=new Vector3(x*generators[i].scale,generators[i].index*generators[i].scale*levelOffset,z*generators[i].scale);
//     var newG=Instantiate(stairs,pos,Quaternion.identity);

//     Destroy(generators[i].piecePlaces[x,z].model);
//     Destroy(generators[i+1].piecePlaces[x,z].model);

//     generators[i].piecePlaces[x,z].model=newG;
//     generators[i+1].piecePlaces[x,z].model=newG;
//     generators[i].piecePlaces[x,z].piece=Map.PieceType.Lader;
//     generators[i+1].piecePlaces[x,z].piece=Map.PieceType.Lader;
//}