using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MapGeneration{
public class Map : MonoBehaviour
{
    [Header("Map Info")]
    [SerializeField] int mapScale=6;
    [SerializeField] float floorOffset;
    [SerializeField] AlgorithmType algorithmType;

    [Header("Regions")]
    [Tooltip("Plese ensure that baseRegion has all Pieces")]
    [SerializeField] RegionConfig baseRegion;
    [SerializeField] RegionConfig[] regions;
    public Event OnMapGenerated;
    Generator[] generators;
    int mapSize,numberOfFloors;
    public static Map instance;
    List<MapPos> bottomEnds=new List<MapPos>();
    List<MapPos> topEnds=new List<MapPos>();
    RegionConfig currentRegion;
    Vector3Int mapEntry,mapExit;
    private void Awake()
    {
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(this);
        }
    }
    void Start()
    {
        baseRegion.SetAsBase();
        if(regions.Length>0){
            regions.Shuffle();
            currentRegion=regions[0];
        }else{
            currentRegion=baseRegion;
        }
        mapSize=UnityEngine.Random.Range(currentRegion.mapMinMaxSize.x,currentRegion.mapMinMaxSize.y);
        numberOfFloors=currentRegion.numberOfFloors;
        generators=new Generator[numberOfFloors];
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i]=new GameObject($"Generator{i}").AddComponent<Generator>();
            generators[i].CreteMap(mapScale,mapSize,currentRegion);
        }
        for (int i = 0; i < generators.Length-1; i++)
        {
            if(PlaceStairs(i,180,MapPieceType.room,MapPieceType.room))continue;
        }
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].MoveToPos(i*floorOffset);
            generators[i].HandleRooms();
        }
        GetMapEntryAndExit();
        PlacePlayer();
        InvokeOnMapGenerated();
    }
    bool PlaceStairs(int i, float rotAngle,MapPieceType lower,MapPieceType upper){
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                if(generators[i].objects[x,z].type==lower){
                    bottomEnds.Add(new MapPos(x,z));
                }
                if(generators[i+1].objects[x,z].type==upper){
                    topEnds.Add(new MapPos(x,z));
                }
            }
        }
        if(bottomEnds.Count==0||topEnds.Count==0)return false;
        MapPos stairsBottom=bottomEnds[UnityEngine.Random.Range(0,bottomEnds.Count-1)];
        MapPos stairsTop=topEnds[UnityEngine.Random.Range(0,topEnds.Count-1)];
        Vector3 posBottom=stairsBottom.ToVector3(mapScale,generators[i].transform.position.y);
        Vector3 posTop=stairsTop.ToVector3(mapScale,generators[i+1].transform.position.y);

        //Remove old room and add stairs
        Destroy(generators[i].objects[stairsBottom.x,stairsBottom.z].pieceObj);
        Destroy(generators[i+1].objects[stairsTop.x,stairsTop.z].pieceObj);

        var stairsGO=Instantiate(currentRegion.GetMapPiece(MapPieceType.stairs).piecePrefab,posBottom,Quaternion.identity);
        stairsGO.transform.Rotate(0,rotAngle,0);
        stairsGO.transform.SetParent(generators[i].transform);

        generators[i].objects[stairsBottom.x,stairsBottom.z].pieceObj=stairsGO;
        generators[i+1].objects[stairsTop.x,stairsTop.z].pieceObj=stairsGO;

        generators[i].objects[stairsBottom.x,stairsBottom.z].type=MapPieceType.stairs;
        generators[i+1].objects[stairsTop.x,stairsTop.z].type=MapPieceType.stairs;
        //Calculate offset
        generators[i+1].xOffset=stairsBottom.x-stairsTop.x+generators[i].xOffset;
        generators[i+1].zOffset=stairsBottom.z-stairsTop.z+generators[i].zOffset;
        generators[i].exit=stairsBottom;
        generators[i+1].entry=stairsTop;

        bottomEnds.Clear();
        topEnds.Clear();
        return true;
}
    #region Events Invokers
    void InvokeOnMapGenerated(){
        if(OnMapGenerated!=null){
            OnMapGenerated();
        }
    }
    void PlacePlayer(){
        var player=GameObject.FindObjectOfType<PlayerControler>();
        
    }
    void GetMapEntryAndExit(){
        Generator generator1=generators[UnityEngine.Random.Range(0,generators.Length-1)];
        MapPos entry=generator1.GetEntryPos();
        mapEntry=new Vector3Int(entry.x,0,entry.z);
    }
    #endregion

}
}