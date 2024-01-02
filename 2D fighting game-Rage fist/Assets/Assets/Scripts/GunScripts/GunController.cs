using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    [Header("Gun parts")]
    public GameObject gun;
    public GameObject muzzle;

    [Header("Gun settings")] // Some settings are found in the enums at the top of the script. Settings will be set to 0 if no gun is being held 
    public GameObject bulletProjectile;
    private GameObject newBulletProjectile;

    public float bulletSpeed;

    #region Setting up new input system
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
    #endregion

    #region Gun system
    private void FixedUpdate()
    {
        // Guided missile lol accidentaly made, also includes rocket missile falling due to gravity
        /*if (GameObject.FindGameObjectWithTag("Bullet"))
        {
            newBulletProjectile.transform.rotation = gun.transform.rotation;
            newBulletProjectile.GetComponent<Rigidbody2D>().AddForce(gun.transform.right * bulletSpeed, ForceMode2D.Impulse);
            Debug.Log(newBulletProjectile.GetComponent<Rigidbody2D>().velocity);
        }*/
    }

    private void Update()
    {
        // Find the gun and muzzle
        gun = GameObject.Find("Gun");
        muzzle = gun.transform.Find("Muzzle").gameObject;

        // Converts the mouse position to world position
        Vector3 mousePosition = playerControls.Player.GunPointer.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, gun.transform.position.z - Camera.main.transform.position.z));

        Vector2 mouseDirection = (mousePosition - transform.position).normalized;

        // Rotates the gun to face the mouse
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void CreateProjectile(ProjectileTypes projectileType)
    {
        switch (projectileType)
        {
            case ProjectileTypes.Bullet:
                newBulletProjectile = Instantiate(bulletProjectile, muzzle.transform.position, gun.transform.rotation);
                newBulletProjectile.transform.rotation = gun.transform.rotation;

                break;

            case ProjectileTypes.Bow:
                // bow
                break;

            case ProjectileTypes.Rocket:
                // rocket
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
                CreateProjectile(ProjectileTypes.Bullet);

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
#endregion
}
