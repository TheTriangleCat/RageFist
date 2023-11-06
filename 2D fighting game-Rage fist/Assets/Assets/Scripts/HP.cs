using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script is for the HP bar and handling HP dealing, and other stuff with Health Points.
*/

/// <summary>
/// This script is for handling the player HP system. Simple as that.
/// No need to set any variables except for damageDealt unless the gameObject is the player. If it is then we have to set all of it.
/// </summary>

public class HP : MonoBehaviour
{
    #region Player Health
    [Header("Player Health")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject healthBar;

    public float currentHp; // Reference variable, current HP sets to max hp at the beginning of the game
    public float maxHp;

    public float shieldDmgReducer; // This will later be turned into a percentage (percent of reduced received damage.)

    public float damageTaken; // The damage you take, resets to 0 every frame, current DMG - damage taken.
    public float damageDealt; // the damage you deal. Can be changed through code, or other scripts
    #endregion

    // Health system
    #region Player health points system
    private void Start()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (gameObject.tag != "DamagePlayer")
        {
            currentHp -= damageTaken;
            damageTaken = 0;

            healthText.text = Mathf.RoundToInt(currentHp / maxHp * 100) + "%";
            healthBar.GetComponent<Slider>().value = currentHp / maxHp;

            if (currentHp <= 0)
            {
                Debug.Log("You died");
                currentHp = 0;
            }

            else if (currentHp > maxHp) 
            {
                Debug.LogWarning("Not suppose to happen, either someone is hacking or the devs are changing it for testing purposes only.");
            }

            if (Input.GetKey(KeyCode.F))
            {
                damageTaken = damageTaken * 100 / (100 - shieldDmgReducer);
                Debug.Log(damageTaken);
            }
        }
    }

    // Detects if the player has been hit or not
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DamagePlayer")
        {
            damageTaken = collision.gameObject.GetComponent<HP>().damageDealt;
        }
    }
    #endregion

}
