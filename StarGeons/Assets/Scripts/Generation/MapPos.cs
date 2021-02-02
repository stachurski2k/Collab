using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPos
{
    public static MapPos[] directions=new MapPos[]{
        new MapPos(1,0),
        new MapPos(0,1),
        new MapPos(-1,0),
        new MapPos(0,-1) 
    };
    public int x,z;
    public MapPos(int x=0,int z=0){
        this.x=x;
        this.z=z;
    }
    public static MapPos operator +(MapPos a,MapPos b){
        return new MapPos(a.x+b.x,a.z+b.z);
    }
    public Vector3 ToVector3(float scale=1,float y=0){
        return new Vector3(x*scale,y,z*scale);
    }
    public Vector2 ToVector2(){
        return new Vector2(x,z);
    }
    public override bool Equals(object obj)
    {
        MapPos pos= obj as MapPos;
        return pos!=null&&pos.x==x&&pos.z==z;
    }
}
