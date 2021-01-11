using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    static System.Random sysR=new System.Random();
    public static void Shuffle<T>(this IList<T> list){
        int n =list.Count;
        while(n>1){
            n--;
            int k=sysR.Next(n+1);
            T value=list[n];
            list[n]=list[k];
            list[k]=value;
        }
    }
}
