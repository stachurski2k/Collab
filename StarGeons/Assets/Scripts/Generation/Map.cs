using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MapGeneration{
public class Map : MonoBehaviour
{
    [Header("Map Info")]
    [SerializeField] int mapSize;
    [SerializeField] int mapScale=6;
    [Range(1,10)]
    [SerializeField] int numberOfFloors=1;
    [SerializeField] float floorOffset;

    [Header("Rooms Info")]
    [SerializeField] int maxRooms;
    [SerializeField] int minRoomSize,maxRoomSize;
    [Header("Regions")]
    [Tooltip("For safety")]
    [SerializeField] RegionConfig baseRegion;
    [SerializeField] RegionConfig[] regions;
    public Event OnMapGenerated;
    Generator[] generators;
    void Start()
    {
        RegionConfig currentRegion;
        if(regions.Length>0){
            currentRegion=regions[0];
            regions.Shuffle();
        }else{
            currentRegion=baseRegion;
        }
        generators=new Generator[numberOfFloors];
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i]=new Generator();
            generators[i].CreteMap(mapScale,mapSize,currentRegion);
        }
        InvokeOnMapGenerated();
    }
    #region Events Invokers
    void InvokeOnMapGenerated(){
        if(OnMapGenerated!=null){
            OnMapGenerated();
        }
    }
    #endregion

}
}