using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapGeneration{
[CreateAssetMenu(menuName="Generation/new Region")]
public class RegionConfig : ScriptableObject
{
    [Header("Map Pieces")]
    [SerializeField] MapPieceConfig[] mapPieces;
    [Header("Room Pieces")]
    [SerializeField] RoomPieceConfig[] roomPieces;

    [Header("Possible Items")]
    [SerializeField] SpawnItemConfig[] possibleItems;
    [Header("Possible enemies")]
    [SerializeField] SpawnEnemyConfig[] possibleEnemies;
    public MapPieceConfig GetMapPiece(MapPieceType type){
        for (int i = 0; i < roomPieces.Length; i++)
        {
            if(mapPieces[i].type==type){
                mapPieces.Shuffle();
                return mapPieces[i];
            }
        }
        return null;
    }
    public GameObject GetRoomPiece(RoomPieceType type){
        for (int i = 0; i < roomPieces.Length; i++)
        {
            if(roomPieces[i].type==type){
                roomPieces.Shuffle();
                return roomPieces[i].piecePrefab;
            }
        }
        return null;
    }
    public GameObject GetRandomEnemy(){
        if(possibleEnemies.Length==0)return null;//no enemies
        return possibleEnemies[Random.Range(0,possibleEnemies.Length-1)].enemy;
    }
    public GameObject GetRandomItem(){
        if(possibleItems.Length==0)return null;
        return possibleItems[Random.Range(0,possibleItems.Length-1)].item;
    }
}
[System.Serializable]
public class MapPieceConfig{
    public MapPieceType type;
    public GameObject piecePrefab;
    [Tooltip("Please adjust the piece forward with z axis")]
    public float defaultRotation;
}
[System.Serializable]
public class RoomPieceConfig{
    public RoomPieceType type;
    public GameObject piecePrefab;
    [Tooltip("Please adjust the piece forward with z axis")]
    public float defaultRotation;
}
[System.Serializable]
public class SpawnItemConfig{
    public float spawnChance;
    //Later change to item config when items sys is ready
    public GameObject item;
}
[System.Serializable]
public class SpawnEnemyConfig{
    // public EnemyType type1;
    public GameObject enemy;
}
}