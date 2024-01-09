using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Making multiple functions for all of the type of weapons example of weapons: bow, rocket, bullet, etc... we can add more if we wanted to...
/// A parameter in the functions will be set to identify wether the weapon is a automatic, burst or semi auto
/// For particles to work, make sure to keep the particles gameobject in the player when making new characters
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
    [SerializeField] private GameObject muzzle;

    [Header("Gun settings")] // Some settings are found in the enums at the top of the script. Settings will be set to 0 if no gun is being held
    public RaycastHit2D projectileRay;
    [SerializeField] private LayerMask layerToIgnore;

    [SerializeField] private Vector2 mouseDirection;
    [SerializeField] private GameObject bulletProjectile;
    public float bulletSpread;
    private GameObject newBulletProjectile;

    public float bulletSpeed;

    [Header("Particles")]
    public ParticleSystem bulletGroundImpact;
    public ParticleSystem explosionGroundImpact;

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

        mouseDirection = (mousePosition - transform.position).normalized;

        // Rotates the gun to face the mouse
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Make sure that all of the gameobject of gun is ignored in the raycast
        for (int i = 0; i < gun.transform.childCount; i++)
        {
            gun.layer = 2;
            gun.transform.GetChild(i).gameObject.layer = 2;
        }
    }

    private void CreateProjectile(ProjectileTypes projectileType)
    {
        switch (projectileType)
        {
            case ProjectileTypes.Bullet:
                newBulletProjectile = Instantiate(
                    bulletProjectile,
                    muzzle.transform.position,
                    Quaternion.Euler(new Vector3(0f, 0f, gun.transform.rotation.eulerAngles.z + Random.Range(-bulletSpread, bulletSpread)))
                );

                projectileRay = Physics2D.Raycast(muzzle.transform.position, newBulletProjectile.transform.right, 1000f, ~layerToIgnore);

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

    private void ShootGun(FiringModes firingMode)
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
