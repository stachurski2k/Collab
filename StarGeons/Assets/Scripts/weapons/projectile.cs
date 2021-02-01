using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [HideInInspector] public float lifeTime;
    [HideInInspector] public int Damage;

    Enemy enemy;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            enemy = other.GetComponentInParent<Enemy>();
        }
        catch (NullReferenceException)
        {
            //have not found enemy (didn't hit him)
        }

        if (enemy)
        {
            enemy.takeHit(Damage);

            //TODO Play VFX
            //TODO Play SFX
        }
        

        // play VFX


        Destroy(gameObject);
    }
}
