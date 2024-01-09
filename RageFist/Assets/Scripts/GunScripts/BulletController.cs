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

        bulletGroundImpact = gunController.GetComponent<GunController>().bulletGroundImpact;
        projectileRay = gunController.GetComponent<GunController>().projectileRay;
        explosionGroundImpact = gunController.GetComponent<GunController>().explosionGroundImpact;

        bulletGroundImpact.Pause(); //make this STOPPPPP PLSSSS SOAP SOAPY MAN
    }

    private void Update()
    {
        gunController = FindObjectOfType<GunController>();

        bulletSpeed = gunController.GetComponent<GunController>().bulletSpeed;
        gun = gunController.GetComponent<GunController>().gun;

        if (TryGetComponent<Bullet>(out Bullet bullet) && gameObject.transform.Find("Sprite").transform.GetComponent<SpriteRenderer>().enabled)
            transform.Translate(bulletSpeed * Time.deltaTime * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Floor>(out Floor floor))
        {
            gameObject.transform.Find("Sprite").transform.GetComponent<SpriteRenderer>().enabled = false; // Disabling the sprite renderer so the particles can be seen
            bulletGroundImpact.Play();

            Invoke(nameof(DestroyBullet), bulletGroundImpact.main.duration);
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
