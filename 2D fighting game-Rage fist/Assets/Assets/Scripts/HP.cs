using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

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
    // Health variables
    public bool isPlayer;// If the gameObject is the player, set to true, else set to false

    public class HiddenFields
    {
        public Gradient healthGradient;
        public TextMeshProUGUI healthText;
        public GameObject healthBar;
        public Image healthBarFill;

        public int currentHp; // Reference variable, current HP sets to max hp at the beginning of the game
        public int maxHp;
        public int damageTaken; // The damage you take, resets to 0 every frame, current DMG - damage taken.
        public int damageDealt; // the damage you deal. Can be changed through code, or other scripts
    }

    [HideInInspector]
    public HiddenFields hiddenFields;

    public int currentHp; 

    //public float shieldDmgReducer; // This will later be turned into a percentage (percent of reduced received damage.)
    #endregion

    // Health system
    #region Player health points system
    private void Start()
    {
        currentHp = hiddenFields.maxHp;
    }

    private void Update()
    {
        if (!gameObject.CompareTag("DamagePlayer"))
        {
            hiddenFields.healthBarFill.color = hiddenFields.healthGradient.Evaluate(hiddenFields.healthBar.GetComponent<Slider>().normalizedValue);

            currentHp -= hiddenFields.damageTaken;
            hiddenFields.damageTaken = 0;

            hiddenFields.healthText.text = Mathf.RoundToInt(currentHp / hiddenFields.maxHp * 100f) + "%";
            hiddenFields.healthBar.GetComponent<Slider>().value = currentHp / hiddenFields.maxHp;

            /*if (currentHp != 0)
            {
                if (currentHp <= 0)
                {
                    Debug.Log("You died");
                    currentHp = 0;
                }

                else if (currentHp > maxHp) 
                {
                    currentHp = maxHp; // Prevent hacking localy so hackers can't change the health
                }

                if (Input.GetKey(KeyCode.F))
                {
                    damageTaken = damageTaken * 100 / (100 - shieldDmgReducer);
                    Debug.Log(damageTaken);
                }
            }*/
        }
    }

    // Detects if the player has been hit or not
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPlayer)
        {
            hiddenFields.maxHp = collision.gameObject.GetComponent<HP>().hiddenFields.damageDealt;
        }
    }
#endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(HP))]
public class HpCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        HP script = (HP)target;

        // draw checkbox for the bool
        script.isPlayer = EditorGUILayout.Toggle("Is Player", script.isPlayer);
        if (script.isPlayer) // if bool is true, show other fields
        {
           // script.iField = EditorGUILayout.ObjectField("I Field", script.iField, typeof(InputField), true) as InputField;
          //  script.Template = EditorGUILayout.ObjectField("Template", script.Template, typeof(GameObject), true) as GameObject;
        }
    }
}
#endif