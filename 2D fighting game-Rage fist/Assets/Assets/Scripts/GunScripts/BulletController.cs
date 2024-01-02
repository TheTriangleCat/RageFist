using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GunController gunController;

    public float bulletSpeed; 

    private void FixedUpdate()
    {
        gunController = FindObjectOfType<GunController>();
        bulletSpeed = gunController.GetComponent<GunController>().bulletSpeed;

        if (gameObject.CompareTag("Bullet"))
            gameObject.transform.Translate(gameObject.transform.right * bulletSpeed);
    }
}
