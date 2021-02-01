using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapGeneration{
[CreateAssetMenu(menuName="Generation/new Region")]
public class RegionConfig : ScriptableObject
{
    [Range(1,10)]
    public int numberOfFloors;
    public Vector2Int mapMinMaxSize;
    public int numberOfRooms;
    public Vector2Int roomMinMaxSize;


    [Header("Map Pieces")]
    [SerializeField] MapPieceConfig[] mapPieces;
    [Header("Room Data")]
    [SerializeField] RoomPieceConfig[] roomPieces;
    [SerializeField] FurnitureConfig[] furniture;

    [Header("Possible Items")]
    public int maxItemsPerRoom=4;
    [SerializeField] SpawnItemConfig[] possibleItems;
    [Header("Possible enemies")]
    public int maxEnemiesPerRoom=4;
    [SerializeField] SpawnEnemyConfig[] possibleEnemies;
    static RegionConfig baseRegion;
    public void SetAsBase(){
        baseRegion=this;
    }
    public MapPieceConfig GetMapPiece(MapPieceType type){
        for (int i = 0; i < mapPieces.Length; i++)
        {
            if((int)mapPieces[i].type==(int)type){
            var PieceToReturn=mapPieces[i];
            mapPieces.Shuffle();
            return PieceToReturn;
            }

        }
        return baseRegion.GetMapPiece(type);
    }
    public RoomPieceConfig GetRoomPiece(RoomPieceType type){
        for (int i = 0; i < roomPieces.Length; i++)
        {
            if(roomPieces[i].type==type){
                var pieceToReturn=roomPieces[i];
                roomPieces.Shuffle();
                return pieceToReturn;
            }
        }
        return baseRegion.GetRoomPiece(type);
    }
    public GameObject GetRandomEnemy(){
        if(possibleEnemies.Length==0)return null;//no enemies
        return possibleEnemies[Random.Range(0,possibleEnemies.Length-1)].enemy;
    }
    public SpawnItemConfig GetRandomItem(){
        if(possibleItems.Length==0)return null;
        return possibleItems[Random.Range(0,possibleItems.Length-1)];
    }
    public GameObject GetRandomFurniture(FurnitureType type){
        for (int i = 0; i < roomPieces.Length; i++)
        {
            if(furniture[i].type==type){
                var pieceToReturn=furniture[i];
                roomPieces.Shuffle();
                return pieceToReturn.furniture;
            }
        }
        return baseRegion.GetRandomFurniture(type);
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
    [Range(0,100)]
    public float spawnChance;
    //Later change to item config when items sys is ready
    public GameObject item;
}
[System.Serializable]
public class SpawnEnemyConfig{
    // public EnemyType type1;
    public GameObject enemy;
}
[System.Serializable]
public class FurnitureConfig{
    public FurnitureType type;
    public GameObject furniture;
}
}