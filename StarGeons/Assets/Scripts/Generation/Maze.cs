using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation       
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

public class Maze : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>() {
                                            new MapLocation(1,0),
                                            new MapLocation(0,1),
                                            new MapLocation(-1,0),
                                            new MapLocation(0,-1) };
    public int width = 30; //x length
    public int depth = 30; //z length
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

    [SerializeField] Transform player;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        Generate();
        AddRooms(3,4,10);
        DrawMap();
        PlacePlayer();
    }

    void InitialiseMap()
    {
        map = new byte[width,depth];
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
    void AddRooms(int count,int minSize,int maxSize){
        for (int i = 0; i < count; i++)
        {
            int startX=Random.Range(3,width-3);
            int startZ=Random.Range(3,depth-3);
            int roomWidth=Random.Range(minSize,maxSize);
            int roomDepth=Random.Range(minSize,maxSize);

            for (int x = startX; x < width-3&&x<startX+roomWidth; x++)
            {
                for (int z = startZ; z < depth-3&&z<startZ+roomDepth; z++)
                {
                    map[x,z]=0;
                }
            }
            
        }
    }

    void DrawMap()
    {
        for (int z = 1; z < depth-1; z++)
            for (int x = 1; x < width-1; x++)
            {
                if (map[x, z] == 1)
                {
                    //Vector3 pos = new Vector3(x * scale, 0, z * scale);
                   //GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //wall.transform.localScale = new Vector3(scale, scale, scale);
                   // wall.transform.position = pos;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 })) //horizontal end piece -|
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, 0, z * scale);
                    block.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 })) //horizontal end piece |-
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 })) //vertical end piece T
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, 0, z * scale);
                    block.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 })) //vertical end piece upside downT
                {
                    GameObject block = Instantiate(endpiece);
                    block.transform.position = new Vector3(x * scale, 0, z * scale);
                    block.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x,z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 })) //vertical straight
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                     GameObject go = Instantiate(straight, pos, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 })) //horizontal straight
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    Instantiate(straight, pos, Quaternion.identity);
                   
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 })) //crossroad
                {
                    GameObject go = Instantiate(crossroad);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 })) //upper left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 })) //upper right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 })) //lower right corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 })) //lower left corner
                {
                    GameObject go = Instantiate(corner);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 })) //tjunc  upsidedown T
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, -90, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 })) //tjunc  T
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 90, 0);
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 })) //tjunc  -|
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                    go.transform.Rotate(0, 180, 0);
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 })) //tjunc  |-
                {
                    GameObject go = Instantiate(tIntersection);
                    go.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if(map[x,z]==0 &&(CountSquareNeighbours(x,z)>=1&&CountDiagonalNeighbours(x,z)>1||CountSquareNeighbours(x,z)>1&&CountDiagonalNeighbours(x,z)>=1)){
                    Vector3 pos=new Vector3(x*scale,0,z*scale);
                    Instantiate(floor,pos,Quaternion.identity);
                    Instantiate(celling,pos,Quaternion.identity);
                    LocateWalls(x,z);
                    if(left){
                        Instantiate(wall,pos,Quaternion.identity);
                    }
                    if(top){
                        var go=Instantiate(wall,pos,Quaternion.identity);
                        go.transform.Rotate(0,90,0);
                    }
                    if(right){
                        var go=Instantiate(wall,pos,Quaternion.identity);
                        go.transform.Rotate(0,180,0);
                    }
                    if(bottom){
                        var go=Instantiate(wall,pos,Quaternion.identity);
                        go.transform.Rotate(0,-90,0);
                    }
                }
                else{
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
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
}
