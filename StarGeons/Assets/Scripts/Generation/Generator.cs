using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapGeneration{
public class Generator
{
    ///Map data
    public MapPieceType[,] map;
    public MapPos entryPos,exitPos;
    public RegionConfig region;
    public Dictionary<MapPos,GameObject> objects=new Dictionary<MapPos, GameObject>();
    int mapSize;
    float scale;
    public void CreteMap(float scale,int mapSize,RegionConfig currentRegion){
        this.mapSize=mapSize;
        this.scale=scale;
        region=currentRegion;
        InitializeMap();
        Draw();
    }
    public void InitializeMap(){
        map= new MapPieceType[mapSize,mapSize];
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                map[x,z]=MapPieceType.room;
            }
        }
    }
    public void Draw(){
        objects.Clear();
        foreach (var piece in objects.Values)
        {
            Object.Destroy(piece);
        }
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                MapPos pos = new MapPos(x,z);
                var pieceConfig=region.GetMapPiece(map[x,z]);
                var piece=Object.Instantiate(pieceConfig.piecePrefab,pos.ToVector3(scale),Quaternion.Euler(0,pieceConfig.defaultRotation,0));
                objects.Add(pos,piece);
            }
        }
    }
}
}