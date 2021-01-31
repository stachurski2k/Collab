using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration{
    public enum MapPieceType{
        straight,
        corner,
        t_junction,
        crossRoad,
        deadEnd,
        room,
        stairs,
        wall,
    }
    public enum RoomPieceType{
        wall,
        celling,
        floor,
        pillar,
        door,
    }
    public enum RoomType{
        treasure,
        enemySpawn,
    }
    public enum FurnitureType{
        desk,
        table,
        wardrobe,
    }
    public enum AlgorithmType{
        prism,
        recursive,
        wilsons,
    }
}