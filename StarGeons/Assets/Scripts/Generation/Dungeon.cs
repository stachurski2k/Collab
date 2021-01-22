using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Recursive
{
    public override void AddRooms(int count, int minSize, int maxSize)
    {
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

  
}
