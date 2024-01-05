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
    public ParticleSystem bulletImpact;
    private RaycastHit2D projectileRay;

    private void Start()
    {
        bulletImpact.Stop();
    }

    private void Update()
    {
        gunController = FindObjectOfType<GunController>();

        bulletSpeed = gunController.GetComponent<GunController>().bulletSpeed;
        gun = gunController.GetComponent<GunController>().gun;

        projectileRay = gunController.GetComponent<GunController>().projectileRay;

        bulletGroundImpact = gunController.GetComponent<GunController>().bulletGroundImpact;
        explosionGroundImpact = gunController.GetComponent<GunController>().explosionGroundImpact;

        StartCoroutine(MoveBullets());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false; // Disabling the sprite renderer so the particles can be seen
            bulletImpact.Play();

            if (!bulletImpact.isPlaying)
                Destroy(gameObject);
        }
    }

    private IEnumerator MoveBullets()
    {
        // Moving the projectiles
        if (CompareTag("Bullet") && gameObject.GetComponent<SpriteRenderer>().enabled)
            transform.Translate(bulletSpeed * Time.deltaTime * Vector2.right);

        yield return true;
    }
}
