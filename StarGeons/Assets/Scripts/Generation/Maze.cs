// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;



// public class Maze : MonoBehaviour
// {
//     public List<MapLocation> directions = new List<MapLocation>() {
//                                             new MapLocation(1,0),
//                                             new MapLocation(0,1),
//                                             new MapLocation(-1,0),
//                                             new MapLocation(0,-1) };

//     public List<MapLocation> pillarLocations = new List<MapLocation>();

//     public int width = 30; //x length
//     public int depth = 30; //z length
//     public byte[,] map;
//     public int scale = 6;

//     public GameObject straight;
//     public GameObject crossroad;
//     public GameObject corner;
//     public GameObject tIntersection;
//     public GameObject endpiece;
//     public GameObject wallpiece;
//     public GameObject floorpiece;
//     public GameObject ceilingpiece;

//     public GameObject pillar;
//     public GameObject door;

//     public GameObject FPC;




//     // Start is called before the first frame update
//     void Start()
//     {
//         InitialiseMap();
//         Generate();
//         AddRooms(5, 4, 10);
//         DrawMap();
//         PlaceFPC();
//     }

//     public virtual void AddRooms(int count, int minSize, int maxSize)
//     {
//         for (int c = 0; c < count; c++)
//         {
//             int startX = Random.Range(3, width - 3);
//             int startZ = Random.Range(3, depth - 3);
//             int roomWidth = Random.Range(minSize, maxSize);
//             int roomDepth = Random.Range(minSize, maxSize);

//             for (int x = startX; x < width - 3 && x < startX + roomWidth; x++)
//             {
//                 for (int z = startZ; z < depth - 3 && z < startZ + roomDepth; z++)
//                 {
//                     map[x, z] = 0;
//                 }
//             }
//         }
//     }

//     void InitialiseMap()
//     {
//         map = new byte[width,depth];
//         piecePlaces = new Pieces[width, depth];
//         for (int z = 0; z < depth; z++)
//             for (int x = 0; x < width; x++)
//             {
//                     map[x, z] = 1;     //1 = wall  0 = corridor
//             }
//     }

//     public virtual void PlaceFPC()
//     {
//         for (int z = 0; z < depth; z++)
//             for (int x = 0; x < width; x++)
//             {
//                 if (map[x, z] == 0)
//                 {
//                     FPC.transform.position = new Vector3(x * scale, 0, z * scale);
//                     return;
//                 }
//             }
//     }

//     public virtual void Generate()
//     {
//         for (int z = 0; z < depth; z++)
//             for (int x = 0; x < width; x++)
//             {
//                if(Random.Range(0,100) < 50)
//                  map[x, z] = 0;     //1 = wall  0 = corridor
//             }
//     }

//     void DrawMap()
//     {
//         for (int z = 0; z < depth; z++)
//             for (int x = 0; x < width; x++)
//             {
//                 if (map[x, z] == 1)
//                 {
//                     //Vector3 pos = new Vector3(x * scale, 0, z * scale);
//                     //GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                     //wall.transform.localScale = new Vector3(scale, scale, scale);
//                     // wall.transform.position = pos;

//                     piecePlaces[x, z].piece = PieceType.Wall;
//                     piecePlaces[x, z].model = null;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 })) //horizontal end piece -|
//                 {
//                     GameObject block = Instantiate(endpiece);
//                     block.transform.position = new Vector3(x * scale, 0, z * scale);
//                     block.transform.Rotate(0, 180, 0);

//                     piecePlaces[x, z].piece = PieceType.DeadToRight;
//                     piecePlaces[x, z].model = block;

//                 }
//                 else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 })) //horizontal end piece |-
//                 {
//                     GameObject block = Instantiate(endpiece);
//                     block.transform.position = new Vector3(x * scale, 0, z * scale);
//                     piecePlaces[x, z].piece = PieceType.DeadToLeft;
//                     piecePlaces[x, z].model = block;

//                 }
//                 else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 })) //vertical end piece T
//                 {
//                     GameObject block = Instantiate(endpiece);
//                     block.transform.position = new Vector3(x * scale, 0, z * scale);
//                     block.transform.Rotate(0, 90, 0);
//                     piecePlaces[x, z].piece = PieceType.DeadEnd;
//                     piecePlaces[x, z].model = block;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 })) //vertical end piece upside downT
//                 {
//                     GameObject block = Instantiate(endpiece);
//                     block.transform.position = new Vector3(x * scale, 0, z * scale);
//                     block.transform.Rotate(0, -90, 0);
//                     piecePlaces[x, z].piece = PieceType.DeadUpsideDown;
//                     piecePlaces[x, z].model = block;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 })) //vertical straight
//                 {
//                     Vector3 pos = new Vector3(x * scale, 0, z * scale);
//                     GameObject go = Instantiate(straight, pos, Quaternion.identity);
//                     go.transform.Rotate(0, 90, 0);
//                     piecePlaces[x, z].piece = PieceType.Vertical_Straight;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 })) //horizontal straight
//                 {
//                     Vector3 pos = new Vector3(x * scale, 0, z * scale);
//                     GameObject go = Instantiate(straight, pos, Quaternion.identity);
//                     piecePlaces[x, z].piece = PieceType.Horizontal_Straight;
//                     piecePlaces[x, z].model = go;

//                 }
//                 else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 })) //crossroad
//                 {
//                     GameObject go = Instantiate(crossroad);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     piecePlaces[x, z].piece = PieceType.Crossroad;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 })) //upper left corner
//                 {
//                     GameObject go = Instantiate(corner);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     go.transform.Rotate(0, 180, 0);
//                     piecePlaces[x, z].piece = PieceType.Left_Up_Corner;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 })) //upper right corner
//                 {
//                     GameObject go = Instantiate(corner);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     go.transform.Rotate(0, 90, 0);
//                     piecePlaces[x, z].piece = PieceType.Right_Up_Corner;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 })) //lower right corner
//                 {
//                     GameObject go = Instantiate(corner);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     piecePlaces[x, z].piece = PieceType.Right_Down_Corner;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 })) //lower left corner
//                 {
//                     GameObject go = Instantiate(corner);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     go.transform.Rotate(0, -90, 0);
//                     piecePlaces[x, z].piece = PieceType.Left_Down_Corner;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 })) //tjunc  upsidedown T
//                 {
//                     GameObject go = Instantiate(tIntersection);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     go.transform.Rotate(0, -90, 0);
//                     piecePlaces[x, z].piece = PieceType.TUpsideDown;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 })) //tjunc  T
//                 {
//                     GameObject go = Instantiate(tIntersection);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     go.transform.Rotate(0, 90, 0);
//                     piecePlaces[x, z].piece = PieceType.T_Junction;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 })) //tjunc  -|
//                 {
//                     GameObject go = Instantiate(tIntersection);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     go.transform.Rotate(0, 180, 0);
//                     piecePlaces[x, z].piece = PieceType.TToRight;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 })) //tjunc  |-
//                 {
//                     GameObject go = Instantiate(tIntersection);
//                     go.transform.position = new Vector3(x * scale, 0, z * scale);
//                     piecePlaces[x, z].piece = PieceType.TToLeft;
//                     piecePlaces[x, z].model = go;
//                 }
//                 else if (map[x, z] == 0 && (CountSquareNeighbours(x, z) > 1 && CountDiagonalNeighbours(x, z) >= 1 ||
//                                             CountSquareNeighbours(x, z) >= 1 && CountDiagonalNeighbours(x, z) > 1))
//                 {
//                     GameObject floor = Instantiate(floorpiece);
//                     floor.transform.position = new Vector3(x * scale, 0, z * scale);

//                     GameObject ceiling = Instantiate(ceilingpiece);
//                     ceiling.transform.position = new Vector3(x * scale, 0, z * scale);

//                     piecePlaces[x, z].piece = PieceType.Room;
//                     piecePlaces[x, z].model = floor;

//                     GameObject pillarCorner;
//                     LocateWalls(x, z);
//                     if (top)
//                     {
//                         GameObject wall1 = Instantiate(wallpiece);
//                         wall1.transform.position = new Vector3(x * scale, 0, z * scale);
               
//                         wall1.name = "Top Wall";

//                         if (map[x + 1, z] == 0 && map[x + 1, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x,z)))
//                         {
//                             pillarCorner = Instantiate(pillar);
//                             pillarCorner.transform.position = new Vector3(x * scale, 0, z * scale);
//                             pillarCorner.name = "Top Right";
//                             pillarLocations.Add(new MapLocation(x, z));
//                             pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
//                         }

//                         if (map[x - 1, z] == 0 && map[x - 1, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z)))
//                         {
//                             pillarCorner = Instantiate(pillar);
//                             pillarCorner.transform.position = new Vector3((x-1) * scale, 0, z * scale);
//                             pillarCorner.name = "Top Left";
//                             pillarLocations.Add(new MapLocation(x - 1, z));
//                             pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
//                         }
//                     }

//                     if (bottom)
//                     {
//                         GameObject wall2 = Instantiate(wallpiece);
//                         wall2.transform.position = new Vector3(x * scale, 0, z * scale);
//                         wall2.transform.Rotate(0, 180, 0);
//                         wall2.name = "Bottom Wall";

//                         if (map[x + 1, z] == 0 && map[x + 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z - 1)))
//                         {
//                             pillarCorner = Instantiate(pillar);
//                             pillarCorner.transform.position = new Vector3(x * scale, 0, ( z - 1) * scale);
//                             pillarCorner.name = "Bottom Right";
//                             pillarLocations.Add(new MapLocation(x, z - 1));
//                             pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
//                         }

//                         if (map[x - 1, z - 1] == 0 && map[x - 1, z] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z - 1)))
//                         {
//                             pillarCorner = Instantiate(pillar);
//                             pillarCorner.transform.position = new Vector3((x - 1) * scale, 0, (z - 1) * scale);
//                             pillarCorner.name = "Bottom Left";
//                             pillarLocations.Add(new MapLocation(x - 1, z - 1));
//                             pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
//                         }
//                     }

//                     if (right)
//                     {
//                         GameObject wall3 = Instantiate(wallpiece);
//                         wall3.transform.position = new Vector3(x * scale, 0, z * scale);
//                         wall3.transform.Rotate(0, 90, 0);
//                         wall3.name = "Right Wall";

//                         if (map[x + 1, z + 1] == 0 && map[x, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z - 1)))
//                         {
//                             pillarCorner = Instantiate(pillar);
//                             pillarCorner.transform.position = new Vector3(x * scale, 0, (z - 1) * scale);
//                             pillarCorner.name = "Right Top";
//                             pillarLocations.Add(new MapLocation(x, z - 1));
//                             pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
//                         }

//                         if (map[x, z - 1] == 0 && map[x + 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x + 1, z - 1)))
//                         {
//                             pillarCorner = Instantiate(pillar);
//                             pillarCorner.transform.position = new Vector3((x + 1) * scale, 0, (z - 1) * scale);
//                             pillarCorner.name = "Right Bottom";
//                             pillarLocations.Add(new MapLocation(x + 1, z - 1));
//                             pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
//                         }
//                     }

//                     if (left)
//                     {
//                         GameObject wall4 = Instantiate(wallpiece);
//                         wall4.transform.position = new Vector3(x * scale, 0, z * scale);
//                         wall4.transform.Rotate(0, -90, 0);
//                         wall4.name = "Left Wall";

//                         if (map[x - 1, z + 1] == 0 && map[x, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z)))
//                         {
//                             pillarCorner = Instantiate(pillar);
//                             pillarCorner.transform.position = new Vector3((x - 1) * scale, 0, z * scale);
//                             pillarCorner.name = "Left Top";
//                             pillarLocations.Add(new MapLocation(x - 1, z));
//                             pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
//                         }

//                         if (map[x - 1, z - 1] == 0 && map[x, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x - 1, z - 1)))
//                         {
//                             pillarCorner = Instantiate(pillar);
//                             pillarCorner.transform.position = new Vector3((x - 1) * scale, 0, (z - 1) * scale);
//                             pillarCorner.name = "Left Bottom";
//                             pillarLocations.Add(new MapLocation(x - 1, z - 1));
//                             pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
//                         }
//                     }


//                 }
//             }

//         for (int z = 0; z < depth; z++)
//             for (int x = 0; x < width; x++)
//             {

//                 GameObject doorway;
//                 LocateDoors(x, z);
//                 if (top)
//                 {
//                     doorway = Instantiate(door);
//                     doorway.transform.position = new Vector3(x * scale, 0, z * scale);
//                     doorway.transform.Rotate(0, 180, 0);
//                     doorway.name = "Top Doorway";
//                     doorway.transform.Translate(0, 0, 0.01f);
//                 }
//                 if (bottom)
//                 {
//                     doorway = Instantiate(door);
//                     doorway.transform.position = new Vector3(x * scale, 0, z * scale);
//                     doorway.name = "Bottom Doorway";
//                     doorway.transform.Translate(0, 0, 0.01f);
//                 }
//                 if (left)
//                 {
//                     doorway = Instantiate(door);
//                     doorway.transform.position = new Vector3(x * scale, 0, z * scale);
//                     doorway.transform.Rotate(0, 90, 0);
//                     doorway.name = "Left Doorway";
//                     doorway.transform.Translate(0, 0, 0.01f);
//                 }
//                 if (right)
//                 {
//                     doorway = Instantiate(door);
//                     doorway.transform.position = new Vector3(x * scale, 0, z * scale);
//                     doorway.transform.Rotate(0, -90, 0);
//                     doorway.name = "Right Doorway";
//                     doorway.transform.Translate(0, 0, 0.01f);
//                 }
//             }
//     }

//     bool top;
//     bool bottom;
//     bool right;
//     bool left;

//     public void LocateWalls(int x, int z)
//     {
//         top = false;
//         bottom = false;
//         right = false;
//         left = false;

//         if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return;
//         if (map[x, z + 1] == 1) top = true;
//         if (map[x, z - 1] == 1) bottom = true;
//         if (map[x + 1, z] == 1) right = true;
//         if (map[x - 1, z] == 1) left = true;
//     }

//     public void LocateDoors(int x, int z)
//     {
//         top = false;
//         bottom = false;
//         right = false;
//         left = false;

//         if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return;
//         if (piecePlaces[x, z + 1].piece != PieceType.Room && piecePlaces[x, z + 1].piece != PieceType.Wall) top = true;
//         if (piecePlaces[x, z - 1].piece != PieceType.Room && piecePlaces[x, z - 1].piece != PieceType.Wall) bottom = true;
//         if (piecePlaces[x + 1, z].piece != PieceType.Room && piecePlaces[x + 1, z].piece != PieceType.Wall) right = true;
//         if (piecePlaces[x - 1, z].piece != PieceType.Room && piecePlaces[x - 1, z].piece != PieceType.Wall) left = true;
//     }

//     bool IsRoom(int x, int z)
//     {
//         return (CountSquareNeighbours(x, z) > 1 && CountDiagonalNeighbours(x, z) >= 1 ||
//                                             CountSquareNeighbours(x, z) >= 1 && CountDiagonalNeighbours(x, z) > 1);
//     }


//     bool Search2D(int c, int r, int[] pattern)
//     {
//         int count = 0;
//         int pos = 0;
//         for (int z = 1; z > -2; z--)
//         {
//             for (int x = -1; x < 2; x++)
//             {
//                 if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)
//                     count++;
//                 pos++;
//             }
//         }
//         return (count == 9);
//     }

//     public int CountSquareNeighbours(int x, int z)
//     {
//         int count = 0;
//         if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
//         if (map[x - 1, z] == 0) count++;
//         if (map[x + 1, z] == 0) count++;
//         if (map[x, z + 1] == 0) count++;
//         if (map[x, z - 1] == 0) count++;
//         return count;
//     }

//     public int CountDiagonalNeighbours(int x, int z)
//     {
//         int count = 0;
//         if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
//         if (map[x - 1, z - 1] == 0) count++;
//         if (map[x + 1, z + 1] == 0) count++;
//         if (map[x - 1, z + 1] == 0) count++;
//         if (map[x + 1, z - 1] == 0) count++;
//         return count;
//     }

//     public int CountAllNeighbours(int x, int z)
//     {
//         return CountSquareNeighbours(x,z) + CountDiagonalNeighbours(x,z);
//     }
// }
