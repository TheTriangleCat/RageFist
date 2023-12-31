using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Making multiple functions for all of the type of weapons example of weapons: bow, rocket, bullet, etc... we can add more if we wanted to...
/// A parameter in the functions will be set to identify wether the weapon is a automatic, burst or semi auto
/// </summary>

public enum FiringModes
{
    SemiAutomatic,
    Automatic,
    Burst
};

public enum ProjectileTypes
{
    Bullet,
    Rocket,
    Bow
};

public class GunController : MonoBehaviour
{
    private PlayerControls playerControls;

    public GameObject bullet;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Shoot.performed += ctx => ShootGun(FiringModes.SemiAutomatic);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }


    private void CreateBullet(ProjectileTypes bulletType)
    {
        switch (bulletType)
        {
            case ProjectileTypes.Bullet:
                Instantiate(bullet, transform.position, transform.rotation);
                bullet.GetComponent<ParticleSystem>().Play();

                break;

            case ProjectileTypes.Rocket:
                // rocket
                break;

            case ProjectileTypes.Bow:
                // bow
                break;

            default:
                break;
        }
    }

    public void ShootGun(FiringModes firingMode)
    {
        switch (firingMode)
        {
            case FiringModes.SemiAutomatic:
                CreateBullet(ProjectileTypes.Bullet);

                break;

            case FiringModes.Automatic:
                // automatic
                break;

            case FiringModes.Burst:
                // burst
                break;

            default:
                break;
        }
    }
}
