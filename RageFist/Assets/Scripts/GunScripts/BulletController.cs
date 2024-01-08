using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GunController gunController;

    [Header("Particles")] // Particles are found and set in the gun controller script
    private ParticleSystem bulletGroundImpact;
    private ParticleSystem explosionGroundImpact;

    [Header("Bullet Settings")]
    private float bulletSpeed;
    private GameObject gun;
    private RaycastHit2D projectileRay;

    private void Start()
    {
        gunController = FindObjectOfType<GunController>();

        //bulletGroundImpact = gunController.GetComponent<GunController>().bulletGroundImpact;
        projectileRay = gunController.GetComponent<GunController>().projectileRay;
        explosionGroundImpact = gunController.GetComponent<GunController>().explosionGroundImpact;

        //bulletGroundImpact.Stop();
    }

    private void Update()
    {
        gunController = FindObjectOfType<GunController>();

        bulletSpeed = gunController.GetComponent<GunController>().bulletSpeed;
        gun = gunController.GetComponent<GunController>().gun;

        if (CompareTag("Bullet") && gameObject.GetComponent<SpriteRenderer>().enabled)
            transform.Translate(bulletSpeed * Time.deltaTime * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false; // Disabling the sprite renderer so the particles can be seen
           // bulletGroundImpact.Play();

            Destroy(gameObject);//bulletGroundImpact.main.duration
        }
    }
}
