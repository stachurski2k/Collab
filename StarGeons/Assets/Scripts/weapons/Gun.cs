using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float fireRate = 15f;

    [Header("VFX")]
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("Bullet Stats")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletDamage = 10f;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifeTime;


    [Header("Config")]
    [SerializeField] Camera fpsCam;

    float nextTimeToFire = 0f;

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        //creat and set up bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<projectile>().lifeTime = bulletLifeTime;
        bullet.GetComponent<projectile>().Damage = bulletDamage;

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bulletRb.AddForce(bulletSpawn.forward * bulletSpeed, ForceMode.Impulse);

    }

}
