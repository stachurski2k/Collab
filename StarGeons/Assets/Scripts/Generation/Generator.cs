using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sys= System;
namespace MapGeneration{
public class Generator:MonoBehaviour
{
    ///Map data
    public byte[,] map;
    public MapPos entryPos,exitPos;
    public RegionConfig currentRegion;
    public PosData[,] objects;
    public List<Room> rooms=new List<Room>();
    public List<MapPos> pillarsPos=new List<MapPos>();
    public MapPos entry,exit;
    int mapSize;
    float scale;
    static int mapExpand=3;
    public float xOffset,zOffset;
    public void CreteMap(float scale,int mapSize,RegionConfig currentRegion){
        this.mapSize=mapSize;
        this.scale=scale;
        this.currentRegion= currentRegion;
        InitializeMap();
        GenerateMap();
        AddRooms(currentRegion.numberOfRooms,currentRegion.roomMinMaxSize.x,currentRegion.roomMinMaxSize.y);
        AddCorridors();
        Draw();
    }
    public void InitializeMap(){
        map= new byte[mapSize,mapSize];
        objects=new PosData[mapSize,mapSize];
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                map[x,z]=1;
            }
        }
    }
    public MapPos GetEntryPos(){
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                if(objects[x,z].type==MapPieceType.deadEnd){
                    return new MapPos(x,z);
                }
            }
        }
        return entryPos;
    }
    public MapPos GetExitPos(){
        for (int x = mapSize-1; x > 1; x--)
        {
            for (int z = mapSize-1; z > 1; z--)
            {
                if(objects[x,z].type==MapPieceType.deadEnd){
                    return new MapPos(x,z);
                }
            }
        }
        return exitPos;
    }
    public void GenerateMap(){
        Generate(2,2);
    }
    void AddCorridors(){
        byte[,] oldMap=map;
        int oldSize=mapSize;

        mapSize+=mapExpand*2;
        map=new byte[mapSize,mapSize];
        InitializeMap();
        for (int x = 0; x < oldSize; x++)
        {
            for (int z = 0; z < oldSize; z++)
            {
                map[x+3,z+3]=oldMap[x,z];
            }
        }
        int xpos;
        int zpos;
        zpos=mapSize-2;
        xpos=Random.Range(3,mapSize-3);
        while(map[xpos,zpos]!=0&&zpos>1){
            map[xpos,zpos]=0;
            zpos--;
        }
        zpos=2;
        xpos=Random.Range(3,mapSize-3);
        while(map[xpos,zpos]!=0&&zpos<mapSize-1){
            map[xpos,zpos]=0;
            zpos++;
        }
            xpos=mapSize-2;
        zpos=Random.Range(3,mapSize-3);
        while(map[xpos,zpos]!=0&&xpos>1){
            map[xpos,zpos]=0;
            xpos--;
        }
        xpos=2;
        zpos=Random.Range(3,mapSize-3);
        while(map[xpos,zpos]!=0&&xpos<mapSize-1){
            map[xpos,zpos]=0;
            xpos++;
        }
    }
    public void AddRooms(int count, int minSize, int maxSize)
    {
        for (int i = 0; i < count; i++)
        {
            var roomType=(RoomType)Random.Range(0,Sys.Enum.GetNames(typeof(RoomType)).Length);

            int startX=Random.Range(4,mapSize-4);
            int startZ=Random.Range(4,mapSize-4);
            int roomWidth=Random.Range(minSize,maxSize);
            int roomDepth=Random.Range(minSize,maxSize);

            int totalWidth=startX+roomWidth;
            totalWidth=(totalWidth<mapSize-1)?roomWidth:mapSize-startX-1;

            int totalDepth=startZ+roomDepth;
            totalDepth=(totalDepth<mapSize-1)?roomDepth:mapSize-startZ-1;

            for (int x = startX; x < mapSize-1&&x<startX+roomWidth; x++)
            {
                for (int z = startZ; z < mapSize-1&&z<startZ+roomDepth; z++)
                {
             
                    map[x,z]=0;
                }
            }
            var room= new Room(roomType,new MapPos(startX,startZ),totalWidth,totalDepth);
            rooms.Add(room);
            
        }
    }
    void Generate(int x, int z)
    {
        if (CountSquareNeighbours(x, z) >= 2){return; } 
        map[x, z] = 0;

        MapPos.directions.Shuffle();

        Generate(x + MapPos.directions[0].x, z + MapPos.directions[0].z);
        Generate(x + MapPos.directions[1].x, z + MapPos.directions[1].z);
        Generate(x + MapPos.directions[2].x, z + MapPos.directions[2].z);
        Generate(x + MapPos.directions[3].x, z + MapPos.directions[3].z);
    }
    public void Draw(){
        var t=transform;
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                MapPos mapPos=new MapPos(x,z);
                PosData data=new PosData();
                if (map[x, z] == 1)
                {
   
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 })) //horizontal end piece -|
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.deadEnd),180);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 })) //horizontal end piece |-
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.deadEnd),0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 })) //vertical end piece T
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.deadEnd),90);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 })) //vertical end piece upside downT
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.deadEnd),-90);
                }
                else if (Search2D(x,z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 })) //vertical straight
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.straight),90);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 })) //horizontal straight
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.straight));
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 })) //crossroad
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.crossRoad));
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 })) //upper left corner
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.corner),180);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 })) //upper right corner
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.corner),90);

                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 })) //lower right corner
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.corner));
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 })) //lower left corner
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.corner),-90);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 })) //tjunc  upsidedown T
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.t_junction),-90);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 })) //tjunc  T
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.t_junction),90);

                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 })) //tjunc  -|
                {
           
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.t_junction),180);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 })) //tjunc  |-
                {
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.t_junction));
                }
                else if(map[x,z]==0 &&(CountSquareNeighbours(x,z)>=1&&CountDiagonalNeighbours(x,z)>1||CountSquareNeighbours(x,z)>1&&CountDiagonalNeighbours(x,z)>=1)){
                    data.UpdateData(t,mapPos.ToVector3(scale),currentRegion.GetMapPiece(MapPieceType.room));
                    AddWalls(mapPos,data);
                }
                objects[x,z]=data;
            }
        }
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                AddDoors(new MapPos(x,z));
            }
        }
    }
    void AddWalls(MapPos pos,PosData data){
        int x=pos.x;
        int z=pos.z;
        if(x<=0||x>=mapSize-1||z<=0||z>=mapSize-1)return;
        if(map[x,z+1]==1){
            data.CreateChild(pos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.wall));
            if(map[x+1,z]==0&&map[x+1,z+1]==0){
                MapPos pillarPos=new MapPos(x,z);
                if(!pillarsPos.Contains(pillarPos)){
                    pillarsPos.Add(pillarPos);
                    data.CreateChild(pillarPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.pillar));
                }
            }
            if(map[x-1,z]==0&&map[x-1,z+1]==0){
                MapPos pillarPos=new MapPos(x-1,z);
                if(!pillarsPos.Contains(pillarPos)){
                    pillarsPos.Add(pillarPos);
                    data.CreateChild(pillarPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.pillar));
                }
            }
        }
        if(map[x+1,z]==1){
            data.CreateChild(pos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.wall),90);
            if(map[x,z+1]==0&&map[x+1,z+1]==0){
                MapPos pillarPos=new MapPos(x,z);
                if(!pillarsPos.Contains(pillarPos)){
                    pillarsPos.Add(pillarPos);
                    data.CreateChild(pillarPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.pillar));
                }
            }
            if(map[x,z-1]==0&&map[x+1,z-1]==0){
                MapPos pillarPos=new MapPos(x,z-1);
                if(!pillarsPos.Contains(pillarPos)){
                    pillarsPos.Add(pillarPos);
                    data.CreateChild(pillarPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.pillar));
                }
            }
        }
        if(map[x,z-1]==1){
            data.CreateChild(pos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.wall),180);
            if(map[x+1,z]==0&&map[x+1,z-1]==0){
                MapPos pillarPos=new MapPos(x,z-1);
                if(!pillarsPos.Contains(pillarPos)){
                    pillarsPos.Add(pillarPos);
                    data.CreateChild(pillarPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.pillar));
                }
            }
            if(map[x-1,z]==0&&map[x-1,z-1]==0){
                MapPos pillarPos=new MapPos(x-1,z-1);
                if(!pillarsPos.Contains(pillarPos)){
                    pillarsPos.Add(pillarPos);
                    data.CreateChild(pillarPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.pillar));
                }
            }
        }
        if(map[x-1,z]==1){
            data.CreateChild(pos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.wall),-90);
            if(map[x,z+1]==0&&map[x-1,z+1]==0){
                MapPos pillarPos=new MapPos(x-1,z);
                if(!pillarsPos.Contains(pillarPos)){
                    pillarsPos.Add(pillarPos);
                    data.CreateChild(pillarPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.pillar));
                }
            }
            if(map[x,z-1]==0&&map[x-1,z-1]==0){
                MapPos pillarPos=new MapPos(x-1,z-1);
                if(!pillarsPos.Contains(pillarPos)){
                    pillarsPos.Add(pillarPos);
                    data.CreateChild(pillarPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.pillar));
                }
            }
        }
    }
    void AddDoors(MapPos mapPos){
        int x=mapPos.x;
        int z=mapPos.z;
        if(objects[x,z].type!=MapPieceType.room){return;}
        if (x <= 0 || x >= mapSize - 1 || z <= 0 || z >= mapSize - 1) return;
        if (objects[x, z + 1].type != MapPieceType.room && objects[x, z + 1].type != MapPieceType.wall){
            objects[x,z].CreateChild(mapPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.door),transform,180);
        }
        if (objects[x, z - 1].type != MapPieceType.room && objects[x, z - 1].type != MapPieceType.wall) {
            objects[x,z].CreateChild(mapPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.door),transform);
        }
        if (objects[x + 1, z].type != MapPieceType.room && objects[x + 1, z].type != MapPieceType.wall){
            objects[x,z].CreateChild(mapPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.door),transform,-90);
        }
        if (objects[x - 1, z].type != MapPieceType.room && objects[x - 1, z].type != MapPieceType.wall) {
            objects[x,z].CreateChild(mapPos.ToVector3(scale),currentRegion.GetRoomPiece(RoomPieceType.door),transform,90);
        }
    }
    public void HandleRooms(){
        List<MapPos> used=new List<MapPos>();
        foreach (var room in rooms)
        {
            for (int x = 0; x < room.width; x++)
            {
                MapPos pos=new MapPos(x+room.roomStart.x+mapExpand,room.depth+room.roomStart.z+mapExpand);
                if(used.Contains(pos)||objects[pos.x,pos.z].type!=MapPieceType.wall)
                {
                    continue;
                }
                used.Add(pos);
                room.CreateThingAt(currentRegion.GetRandomFurniture(FurnitureType.wardrobe),transform.position,x,room.depth-1,scale,180);
            }
            for (int x = 0; x < room.width; x++)
            {
                MapPos pos=new MapPos(x+room.roomStart.x+mapExpand,room.roomStart.z+mapExpand-1);
                if(used.Contains(pos)||objects[pos.x,pos.z].type!=MapPieceType.wall)
                {
                    continue;
                }
                used.Add(pos);
                room.CreateThingAt(currentRegion.GetRandomFurniture(FurnitureType.wardrobe),transform.position,x,0,scale);
            }
            for (int z = 0; z < room.depth; z++)
            {
                MapPos pos=new MapPos(room.roomStart.x+mapExpand-1,z+room.roomStart.z+mapExpand);
                if(used.Contains(pos)||objects[pos.x,pos.z].type!=MapPieceType.wall)
                {
                    continue;
                }
                used.Add(pos);
                room.CreateThingAt(currentRegion.GetRandomFurniture(FurnitureType.wardrobe),transform.position,0,z,scale,90);
            }
            for (int z = 0; z < room.depth; z++)
            {
                MapPos pos=new MapPos(room.roomStart.x+mapExpand+room.width,z+room.roomStart.z+mapExpand);
                if(used.Contains(pos)||objects[pos.x,pos.z].type!=MapPieceType.wall)
                {
                    continue;
                }
                used.Add(pos);
                room.CreateThingAt(currentRegion.GetRandomFurniture(FurnitureType.wardrobe),transform.position,room.width-1,z,scale,-90);
            }
            for (int x = 1; x < room.width-1; x++)
            {
                for (int z = 1; z < room.depth-1; z++)
                {
                    MapPos pos=new MapPos(room.roomStart.x+mapExpand+x,room.roomStart.z+mapExpand+z);
                    if(used.Contains(pos)||Random.Range(0,1f)<.85f){
                        continue;
                    }
                    used.Add(pos);
                    room.CreateThingAt(currentRegion.GetRandomFurniture(FurnitureType.table),transform.position,x,z,scale);
                }
            }
            //items
            if(room.roomType==RoomType.treasure){
                for (int x = 1; x < room.width-1; x++)
                {
                    for (int z = 1; z < room.depth-1; z++)
                    {
                        MapPos pos=new MapPos(x+room.roomStart.x+mapExpand,z+room.roomStart.z+mapExpand);
                        if(!used.Contains(pos)){
                            var item=currentRegion.GetRandomItem();
                            if(UnityEngine.Random.Range(0,100f)<item.spawnChance){
                                room.CreateThingAt(item.item,transform.position,x,z,scale);
                                used.Add(pos);
                            }
                        }
                    }
                }
            }
            else if(room.roomType==RoomType.enemySpawn){
                  for (int x = 1; x < room.width-1; x++)
                {
                    for (int z = 1; z < room.depth-1; z++)
                    {
                        MapPos pos=new MapPos(x+room.roomStart.x+mapExpand,z+room.roomStart.z+mapExpand);
                        if(!used.Contains(pos)){
                            room.CreateThingAt(currentRegion.GetRandomEnemy(),transform.position,x,z,scale);
                            used.Add(pos);
                        }
                    }
                }
            }
        }
    }
    #region GeneratingMethods
    bool Search2D(int c, int r, int[] pattern)
    {
        int count = 0;
        int pos = 0;
        for (int z = 1; z > -2; z--)
        {
            for (int x = -1; x < 2; x++)
            {
                if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)
                    count++;
                pos++;
            }
        }
        return (count == 9);
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= mapSize - 1 || z <= 0 || z >= mapSize - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= mapSize - 1 || z <= 0 || z >= mapSize - 1) return 5;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        return count;
    }

    public int CountAllNeighbours(int x, int z)
    {
        return CountSquareNeighbours(x,z) + CountDiagonalNeighbours(x,z);
    }
    
    #endregion
    public void MoveToPos(float height){
        transform.position=new Vector3(xOffset*scale,height*scale,zOffset*scale);
    }
    
    public class Room{
    public RoomType roomType;
    public bool[,] roomMap;
    public MapPos roomStart;
    public int width,depth;
    public Room(RoomType type,MapPos startPos,int roomWidth,int roomDepth){
        roomMap=new bool[roomWidth,roomDepth];
        roomType=type;
        roomStart=startPos;
        width=roomWidth;
        depth=roomDepth;
    }
    public void CreateThingAt(GameObject prefab,Vector3 parent,int x,int z,float scale,float rotation=0){
        Vector3 pos=new Vector3((roomStart.x+mapExpand+x)*scale,0,(roomStart.z+mapExpand+z)*scale)+parent;
        GameObject.Instantiate(prefab,pos,Quaternion.Euler(0,rotation,0));
    }
    
}
}
public class PosData{
    public MapPieceType type;
    public GameObject pieceObj;
    public List<GameObject> childs=new List<GameObject>();
    public PosData(){
        type=MapPieceType.wall;
    }
    public void CreateChild(Vector3 pos,RoomPieceConfig prefab,float rotation=0){
        var newChild=(GameObject.Instantiate(prefab.piecePrefab,pos,Quaternion.Euler(0,rotation+prefab.defaultRotation,0)));
        newChild.transform.SetParent(pieceObj.transform);
        childs.Add(newChild);
    }
      public void CreateChild(Vector3 pos,RoomPieceConfig prefab,Transform parent,float rotation=0){
        var newChild=(GameObject.Instantiate(prefab.piecePrefab,pos,Quaternion.Euler(0,rotation+prefab.defaultRotation,0)));
        newChild.transform.SetParent(parent);
        childs.Add(newChild);
    }
    public void UpdateData(Transform parent,Vector3 pos,MapPieceConfig piecePrefab,float rotation=0){
        this.type=piecePrefab.type;
        pieceObj=GameObject.Instantiate(piecePrefab.piecePrefab,pos,Quaternion.Euler(0,piecePrefab.defaultRotation+rotation,0));
        pieceObj.transform.SetParent(parent);
        pieceObj.name=piecePrefab.type.ToString();
    }
}
}