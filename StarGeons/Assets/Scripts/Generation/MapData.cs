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
    }
    public enum RoomPieceType{
        wall,
        celling,
        floor,
    }
}