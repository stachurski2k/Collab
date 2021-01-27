using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [HideInInspector] public float lifeTime;
    [HideInInspector] public float Damage;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"hit {other.transform.name}");
        Destroy(gameObject);

        // do things

        // play VFX
    }
}
