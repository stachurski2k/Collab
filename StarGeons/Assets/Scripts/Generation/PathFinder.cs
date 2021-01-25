using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
public class PathMarker{
    public MapLocation location;
    public float G,H,F;
    public PathMarker parent;
    public PathMarker(MapLocation l,float g,float h,float f,PathMarker p){
        G=g;
        H=h;
        F=f;
        location=l;
        parent=p;
    }
    public override bool Equals(object obj)
    {
        PathMarker pM=obj as PathMarker;
        Debug.Log(pM!=null&&pM.location.Equals(location));
        return pM!=null&&pM.location.Equals(location);
    }
}
public class PathFinder : MonoBehaviour
{
    public Map map;
    List<PathMarker> openMarkers=new List<PathMarker>();
    List<PathMarker> closedMarkers=new List<PathMarker>();
    PathMarker goalMarker;
    PathMarker startMarker;
    PathMarker lastPos;
    bool done;
    public void Build()
    {
        BeginSerch();
        while(!done){
            Search(lastPos);
        }
        map.InitialiseMap();
        GetPath();
    }

    // void Update()
    // {
    //     if(Keyboard.current.pKey.wasPressedThisFrame){
    //         DeleteAllMarkers();
    //         BeginSerch();
    //     }
    //     if(Keyboard.current.cKey.wasPressedThisFrame&&!done){
    //         Search(lastPos);
    //     }
    // }
    void BeginSerch(){
        done=false;
        List<MapLocation> locations=new List<MapLocation>();
        for (int x = 1; x < map.width-1; x++)
        {
            for (int z = 1; z < map.depth-1; z++)
            {
                if(map.map[x,z]!=1){
                    locations.Add(new MapLocation(x,z));
                }
            }
        }
        locations.Shuffle();
        startMarker=new PathMarker(locations[0],0,0,0,null);
       goalMarker=new PathMarker(locations[1],0,0,0,null);
        openMarkers.Clear();
        closedMarkers.Clear();
        openMarkers.Add(startMarker);
        lastPos=startMarker;
    }
    void GetPath(){
        PathMarker pos=lastPos;
        List<PathMarker> path=new List<PathMarker>();
        while(pos!=null){
            path.Add(pos);
            pos=pos.parent;
        }
       foreach (var item in path)
       {
           map.map[item.location.x,item.location.z]=0;
       }
        // foreach(var pathPos in path){
        //     pathPos.marker.transform.GetComponent<MeshRenderer>().material.color=Color.green;
        // }
    }
    void Search(PathMarker marker){
        if(marker.Equals(goalMarker)){done=true;
        return;}
        foreach (var dir in map.directions)
        {
            var neighbour=dir+marker.location;
            if(map.map[neighbour.x,neighbour.z]==1)continue;
            if(neighbour.x<1||neighbour.x>=map.width||neighbour.z<1||neighbour.z>=map.depth)continue;
            if(IsClosed(neighbour))continue;
            float G=Vector2.Distance(marker.location.ToVector2(),neighbour.ToVector2())+marker.G;
            float H=Vector2.Distance(neighbour.ToVector2(),goalMarker.location.ToVector2());
            float F=G+H;
            //var pathBlock=Instantiate(visitedOBJ,neighbour.ToVector3(map.scale,map.height),Quaternion.identity);
            
            if(!UpdateMarker(neighbour,G,H,F,marker)){
                openMarkers.Add(new PathMarker(neighbour,G,H,F,marker));
            }
        }
        openMarkers=openMarkers.OrderBy(p=>p.F).ToList<PathMarker>();
        PathMarker pathMarker=openMarkers[0];
        closedMarkers.Add(pathMarker);
        openMarkers.RemoveAt(0);
        lastPos=pathMarker;
    }
    bool UpdateMarker(MapLocation pos, float g, float h, float f,PathMarker marker){
        foreach (var m in openMarkers)
        {
            if(m.location.Equals(pos)){
                m.G=g;
                m.H=h;
                m.F=f;
                m.parent=marker;
                return true;
            }
        }
        return false;
    }
    bool IsClosed(MapLocation marker){
        foreach (var item in closedMarkers)
        {
            if(item.location.Equals(marker))return true;
        }
        return false;
    }
}
