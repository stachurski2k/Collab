using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPos
{
    public int x,z;
    public MapPos(int x=0,int z=0){
        this.x=x;
        this.z=z;
    }
    public Vector3 ToVector3(float scale=1){
        return new Vector3(x*scale,0,z*scale);
    }
}
