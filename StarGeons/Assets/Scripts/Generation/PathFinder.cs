using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;
namespace MapGeneration{
public class PathMarker{
    public MapPos location;
    public float G,H,F;
    public PathMarker parent;
    public PathMarker(MapPos l,float g,float h,float f,PathMarker p){
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
    public static PathFinder instance;
    private void Awake()
    {
        if(instance==null){
            instance=this;
        }else{
            Destroy(this);
        }
    }
    public static void BeginBuild(Generator _map,MapPos start,MapPos end,Action<List<PathMarker>> pathHandler){
        ThreadManager.ExecuteOnNewThread<List<PathMarker>>(()=>{
            return Search(_map,start,end);
        },pathHandler);
            
    }
    static List<PathMarker> Search(Generator _map,MapPos start,MapPos end){
        List<MapPos> locations=new List<MapPos>();
            for (int x = 1; x < _map.mapSize-1; x++)
            {
                for (int z = 1; z < _map.mapSize-1; z++)
                {
                    if(_map.map[x,z]!=1){
                        locations.Add(new MapPos(x,z));
                    }
                }
            }
            PathMarker startMarker=new PathMarker(start,0,0,0,null);
            PathMarker goalMarker=new PathMarker(end,0,0,0,null);
            List<PathMarker> openMarkers=new List<PathMarker>();
            List<PathMarker> closedMarkers=new List<PathMarker>();
            PathMarker lastPos=startMarker;
            openMarkers.Add(startMarker);

            bool done=false;
            while(!done){
                if(lastPos.Equals(goalMarker)){
                    done=true;break;
                }
                foreach (var dir in MapPos.directions)
                {
                    var neighbour=dir+lastPos.location;
                    if(_map.map[neighbour.x,neighbour.z]==1)continue;
                    if(neighbour.x<1||neighbour.x>=_map.mapSize||neighbour.z<1||neighbour.z>=_map.mapSize)continue;
                    if(IsClosed(neighbour,closedMarkers))continue;
                    float G=Vector2.Distance(lastPos.location.ToVector2(),neighbour.ToVector2())+lastPos.G;
                    float H=Vector2.Distance(neighbour.ToVector2(),goalMarker.location.ToVector2());
                    float F=G+H;
                    //var pathBlock=Instantiate(visitedOBJ,neighbour.ToVector3(map.scale,map.height),Quaternion.identity);
                    
                    if(!UpdateMarker(neighbour,G,H,F,lastPos,openMarkers)){
                        openMarkers.Add(new PathMarker(neighbour,G,H,F,lastPos));
                    }
                }
                openMarkers=openMarkers.OrderBy(p=>p.F).ToList<PathMarker>();
                PathMarker pathMarker=openMarkers[0];
                closedMarkers.Add(pathMarker);
                openMarkers.RemoveAt(0);
                lastPos=pathMarker;
            }
            return GetPath(lastPos);
    }
    static List<PathMarker> GetPath(PathMarker lastPos){
        PathMarker pos=lastPos;
        List<PathMarker> path=new List<PathMarker>();
        while(pos!=null){
            path.Add(pos);
            pos=pos.parent;
        }
        return path;
    }
    static bool UpdateMarker(MapPos pos, float g, float h, float f,PathMarker marker,List<PathMarker> openMarkers){
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
    static bool IsClosed(MapPos marker,List<PathMarker> closedMarkers){
        foreach (var item in closedMarkers)
        {
            if(item.location.Equals(marker))return true;
        }
        return false;
    }
}
}