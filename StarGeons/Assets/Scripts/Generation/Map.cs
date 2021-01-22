using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation       
{
    public static bool HasPos(List<MapLocation> list,MapLocation entity){
        foreach(var pos in list){
            if(pos.x==entity.x&&pos.z==entity.z){
                return true;
            }
        }
        return false;
    }
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
    public Vector3 ToVector3(int scale,float y){
        return new Vector3(x*scale,y,z*scale);
    }   
}

public class Map : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>() {
                                            new MapLocation(1,0),
                                            new MapLocation(0,1),
                                            new MapLocation(-1,0),
                                            new MapLocation(0,-1) };
    List<MapLocation> pillarsLocation=new List<MapLocation>();
    public int width = 30; //x length
    public int depth = 30; //z length
    public int numOfRooms=4;
    public int maxRoomSize=5;
    public byte[,] map;
    public int scale = 6;

    public GameObject straight;
    public GameObject crossroad;
    public GameObject corner;
    public GameObject tIntersection;
    public GameObject endpiece;
    public GameObject floor;
    public GameObject celling;
    public GameObject wall;
    public GameObject pillar;
    public GameObject doorway;
    // public static int nextIndex=0;
    public static bool placedPlayer=false;
    public int index;
    public float levelOffset;
    public float xOffset=0,zOffset=0;
    public float height;
    [SerializeField] Transform player;
    public enum PieceType
    {
        Horizontal_Straight,
        Vertical_Straight,
        Right_Up_Corner,
        Right_Down_Corner,
        Left_Up_Corner,
        Left_Down_Corner,
        T_Junction,
        TUpsideDown,
        TToLeft,
        TToRight,
        DeadEnd,
        DeadUpsideDown,
        DeadToRight,
        DeadToLeft,
        Wall,
        Crossroad,
        Room,
        Lader,
    }

    public struct Pieces
    {
        public PieceType piece;
        public GameObject model;

        public Pieces(PieceType pt, GameObject m)
        {
            piece = pt;
            model = m;
        }
    }

    public Pieces[,] piecePlaces;
    // Start is called before the first frame update
    public void Build(int mapSize,int level)
    {
        index=level;
        width=mapSize;
        depth=mapSize;
        InitialiseMap();
        Generate();
        AddRooms(numOfRooms,2,maxRoomSize);
        DrawMap();
        if(placedPlayer)return;
        PlacePlayer();
        placedPlayer=true;
    }

    void InitialiseMap()
    {
        map = new byte[width,depth];
        piecePlaces=new Pieces[width,depth];
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                    map[x, z] = 1;     //1 = wall  0 = corridor
            }
    }

    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
               if(Random.Range(0,100) < 50)
                 map[x, z] = 0;     //1 = wall  0 = corridor
            }
    }
    public virtual void AddRooms(int count,int minSize,int maxSize){
      
    }

    void DrawMap()
    {
        height=index*scale*levelOffset;
        for (int z = 1; z < depth-1; z++)
            for (int x = 1; x < width-1; x++)
            {
                if (map[x, z] == 1)
                {
                    piecePlaces[x, z].piece = PieceType.Wall;
                    piecePlaces[x, z].model = null;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 })) //horizontal end piece -|
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, height, z * scale);
                    block.transform.Rotate(0, 180, 0);
                    piecePlaces[x, z].piece = PieceType.DeadToRight;
                    block.name="EndRIGht";
                    piecePlaces[x, z].model = block;
                    block.transform.SetParent(gameObject.transform);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 })) //horizontal end piece |-
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, height, z * scale);
                    piecePlaces[x, z].piece = PieceType.DeadToLeft;
                    piecePlaces[x, z].model = block;
                    block.transform.SetParent(gameObject.transform);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 })) //vertical end piece T
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, height, z * scale);
                    block.transform.Rotate(0, 90, 0);
                    piecePlaces[x, z].piece = PieceType.DeadEnd;
                    piecePlaces[x, z].model = block;
                    block.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 })) //vertical end piece upside downT
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, height, z * scale);
                    block.transform.Rotate(0, -90, 0);
                    piecePlaces[x, z].piece = PieceType.DeadUpsideDown;
                    piecePlaces[x, z].model = block;
                    block.transform.SetParent(gameObject.transform);
                    
                }
                else if (Search2D(x,z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 })) //vertical straight
                {
                    Vector3 pos = new Vector3(x * scale, height, z * scale);
                     GameObject go = Instantiate(straight, pos, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);
                    piecePlaces[x, z].piece = PieceType.Vertical_Straight;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 })) //horizontal straight
                {
                    Vector3 pos = new Vector3(x * scale, height, z * scale);
                    GameObject go=Instantiate(straight, pos, Quaternion.identity);
                    piecePlaces[x, z].piece = PieceType.Horizontal_Straight;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                   
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 })) //crossroad
                {
                    GameObject go = Instantiate(crossroad);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    piecePlaces[x, z].piece = PieceType.Crossroad;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 })) //upper left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(0, -180, 0);
                    go.name="rightupper";
                    piecePlaces[x, z].piece = PieceType.Right_Up_Corner;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 })) //upper right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(0, 90, 0);
                    piecePlaces[x, z].piece = PieceType.Left_Up_Corner;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 })) //lower right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    piecePlaces[x, z].piece = PieceType.Left_Down_Corner;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                    
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 })) //lower left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(0, -90, 0);
                    piecePlaces[x, z].piece = PieceType.Right_Down_Corner;
                    piecePlaces[x, z].model = go;
                    go.name="rightdown";
                    go.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 })) //tjunc  upsidedown T
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(0, -90, 0);
                     piecePlaces[x, z].piece = PieceType.TUpsideDown;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 })) //tjunc  T
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(0, 90, 0);
                    piecePlaces[x, z].piece = PieceType.T_Junction;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 })) //tjunc  -|
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                    go.transform.Rotate(0, 180, 0);
                      piecePlaces[x, z].piece = PieceType.TToRight;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 })) //tjunc  |-
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, height, z * scale);
                      piecePlaces[x, z].piece = PieceType.TToLeft;
                    piecePlaces[x, z].model = go;
                    go.transform.SetParent(gameObject.transform);

                }
                else if(map[x,z]==0 &&(CountSquareNeighbours(x,z)>=1&&CountDiagonalNeighbours(x,z)>1||CountSquareNeighbours(x,z)>1&&CountDiagonalNeighbours(x,z)>=1)){
                    Vector3 pos=new Vector3(x*scale,height,z*scale);
                    GameObject f=Instantiate(floor,pos,Quaternion.identity);
                    f.transform.SetParent(gameObject.transform);
                    piecePlaces[x,z].model=f;
                    piecePlaces[x,z].piece=PieceType.Room;
                    var cell=Instantiate(celling,pos,Quaternion.identity);
                    cell.transform.SetParent(gameObject.transform);
                    
                    LocateWalls(x,z);
                    if(left){
                        var w=Instantiate(wall,pos,Quaternion.identity);
                        w.transform.SetParent(gameObject.transform);
                        GameObject pillarConer;
                        if(map[x,z+1]==0&&map[x-1,z+1]==0){
                            MapLocation pilarPos=new MapLocation(x-1,z);
                            if(!MapLocation.HasPos(pillarsLocation,pilarPos)){
                                pillarsLocation.Add(pilarPos);
                                pillarConer=Instantiate(pillar,new Vector3(pilarPos.x*scale,height,pilarPos.z*scale),Quaternion.identity);
                                pillarConer.name="Left Top";
                                pillarConer.transform.SetParent(gameObject.transform);

                            }
                        }
                        if(map[x,z-1]==0&&map[x-1,z-1]==0){
                            MapLocation pilarPos=new MapLocation(x-1,z-1);
                            if(!MapLocation.HasPos(pillarsLocation,pilarPos)){
                                pillarsLocation.Add(pilarPos);
                                pillarConer=Instantiate(pillar,new Vector3(pilarPos.x*scale,height,pilarPos.z*scale),Quaternion.identity);
                                pillarConer.name="Left Btn";
                                pillarConer.transform.SetParent(gameObject.transform);
                            }
                        }
                    }
                    if(top){
                        var w=Instantiate(wall,pos,Quaternion.identity);
                        w.transform.SetParent(gameObject.transform);
                        w.transform.Rotate(0,90,0);
                        GameObject pillarConer;
                        if(map[x+1,z]==0&&map[x+1,z+1]==0){
                            MapLocation pilarPos=new MapLocation(x,z);
                            if(!MapLocation.HasPos(pillarsLocation,pilarPos)){
                                pillarsLocation.Add(pilarPos);
                                pillarConer=Instantiate(pillar,new Vector3(x*scale,height,z*scale),Quaternion.identity);
                                pillarConer.name="Top Right";
                                pillarConer.transform.SetParent(gameObject.transform);
                                
                            }
                        }
                        //
                        if(map[x-1,z]==0&&map[x-1,z+1]==0){
                            MapLocation pilarPos=new MapLocation(x-1,z);
                            if(!MapLocation.HasPos(pillarsLocation,pilarPos)){
                                pillarsLocation.Add(pilarPos);
                                pillarConer=Instantiate(pillar,new Vector3(pilarPos.x*scale,height,z*scale),Quaternion.identity);
                                pillarConer.name="To Left";
                                pillarConer.transform.SetParent(gameObject.transform);

                            }
                        }
                    }
                    if(right){
                        var w=Instantiate(wall,pos,Quaternion.identity);
                        w.transform.SetParent(gameObject.transform);
                        w.transform.Rotate(0,180,0);
                        GameObject pillarConer;
                        if(map[x,z+1]==0&&map[x+1,z+1]==0){
                            MapLocation pilarPos=new MapLocation(x,z);
                            if(!MapLocation.HasPos(pillarsLocation,pilarPos)){
                                pillarsLocation.Add(pilarPos);
                                pillarConer=Instantiate(pillar,new Vector3(x*scale,height,z*scale),Quaternion.identity);
                                pillarConer.name="Right Top";
                                pillarConer.transform.SetParent(gameObject.transform);

                            }
                            
                        }
                        //
                        if(map[x,z-1]==0&&map[x+1,z-1]==0){
                            MapLocation pilarPos=new MapLocation(x,z-1);
                            if(!MapLocation.HasPos(pillarsLocation,pilarPos)){
                                pillarsLocation.Add(pilarPos);
                                pillarConer=Instantiate(pillar,new Vector3(x*scale,height,pilarPos.z*scale),Quaternion.identity);
                                pillarConer.name="Right Btn";
                                pillarConer.transform.SetParent(gameObject.transform);

                            }
                        }
                    }
                    if(bottom){
                        var w=Instantiate(wall,pos,Quaternion.identity);
                        w.transform.SetParent(gameObject.transform);
                        w.transform.Rotate(0,-90,0);
                        GameObject pillarConer;
                        if(map[x+1,z]==0&&map[x+1,z-1]==0){
                            MapLocation pilarPos=new MapLocation(x,z-1);
                            if(!MapLocation.HasPos(pillarsLocation,pilarPos)){
                                pillarsLocation.Add(pilarPos);
                                pillarConer=Instantiate(pillar,new Vector3(x*scale,height,(z-1)*scale),Quaternion.identity);
                                pillarConer.name="Bt Right";
                                pillarConer.transform.SetParent(gameObject.transform);

                            }
                        }
                        if(map[x-1,z]==0&&map[x-1,z-1]==0){
                            MapLocation pilarPos=new MapLocation(x-1,z-1);
                            if(!MapLocation.HasPos(pillarsLocation,pilarPos)){
                                pillarsLocation.Add(pilarPos);
                                pillarConer=Instantiate(pillar,new Vector3((x-1f)*scale,height,(z-1)*scale),Quaternion.identity);
                                pillarConer.name="Bt Left";
                                pillarConer.transform.SetParent(gameObject.transform);

                            }
                      
                        }
                    }
                }
                else{
                    Vector3 pos = new Vector3(x * scale, height, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }


            }
        for (int z = 0; z < depth-1; z++)
        {
            for (int x = 0; x < width-1; x++)
            {
                if(piecePlaces[x,z].piece!=PieceType.Room)continue;
                LocateDoors(x,z);
                GameObject door;
                Vector3 pos=new Vector3(x*scale,height,z*scale);
                if(bottom){
                    door=Instantiate(doorway,pos,Quaternion.identity);
                    door.name="bottom door";
                    door.transform.SetParent(gameObject.transform);
                }
                if(top){
                    door=Instantiate(doorway,pos,Quaternion.identity);
                    door.transform.Rotate(0,180,0);
                    door.name="top door";
                    door.transform.SetParent(gameObject.transform);

                }
                if(right){
                    door=Instantiate(doorway,pos,Quaternion.identity);
                    door.transform.Rotate(0,-90,0);
                    door.name="right door";
                    door.transform.SetParent(gameObject.transform);

                }
                if(left){
                    door=Instantiate(doorway,pos,Quaternion.identity);
                    door.transform.Rotate(0,90,0);
                    door.name="left door";
                    door.transform.SetParent(gameObject.transform);

                }
            }
        }
    }
    bool top;
    bool bottom;
    bool right;
    bool left;
    void LocateWalls(int x,int z){
        top=false;
        bottom=false;
        right=false;
        left=false;
        if(x<=0||x>=width-1||z<=0||z>=depth-1)return;
        if(map[x,z+1]==1){
            top=true;
        }
        if(map[x+1,z]==1){
            right=true;
        }
        if(map[x,z-1]==1){
            bottom=true;
        }
        if(map[x-1,z]==1){
            left=true;
        }
    }
    void LocateDoors(int x,int z){
        top=false;
        bottom=false;
        right=false;
        left=false;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return;
        if (piecePlaces[x, z + 1].piece != PieceType.Room && piecePlaces[x, z + 1].piece != PieceType.Wall) top = true;
        if (piecePlaces[x, z - 1].piece != PieceType.Room && piecePlaces[x, z - 1].piece != PieceType.Wall) bottom = true;
        if (piecePlaces[x + 1, z].piece != PieceType.Room && piecePlaces[x + 1, z].piece != PieceType.Wall) right = true;
        if (piecePlaces[x - 1, z].piece != PieceType.Room && piecePlaces[x - 1, z].piece != PieceType.Wall) left = true;
    }


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
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
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
    public virtual void PlacePlayer(){
        for (int z = 0; z < depth; z++){
            for (int x = 0; x < width; x++)
            {
                if(map[x,z]==0){
                    player.position=new Vector3(x*scale,0,z*scale);
                    return;
                } 
            }
        }
    }
    public void MoveToOffsetPos(){
        transform.position=new Vector3(xOffset*scale,0,zOffset*scale);
    }
}
