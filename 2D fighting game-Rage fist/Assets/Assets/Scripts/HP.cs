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
    #region Variables
    // Health variables
    public bool isPlayer;
    public int damageDealt; // Amount of damage the player or object deals

    [Header("Health Settings")]
    [SerializeField]
    private Gradient healthGradient;

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private GameObject healthBar;

    [SerializeField]
    private Image healthBarFill;

    [Header("Player Settings")]
    [SerializeField]
    [HideInInspector]
    private int currentHp; //Current hp of player

    [SerializeField]
    [HideInInspector]
    private int maxHp; //Max hp of player

    [SerializeField]
    [HideInInspector]
    private int damageTaken; // The damage you take, resets to 0 every frame, current 

    //public float shieldDmgReducer; // This will later be turned into a percentage (percent of reduced received damage.)
    #endregion

    // Health system
    #region Player health points system
    private void Start()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (isPlayer )
        {
            healthBarFill.color = healthGradient.Evaluate(healthBar.GetComponent<Slider>().normalizedValue);

            currentHp -= damageTaken;
            damageTaken = 0;

            healthText.text = Mathf.RoundToInt(currentHp / maxHp * 100f) + "%";
            healthBar.GetComponent<Slider>().value = currentHp / maxHp;

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
            //maxHp = collision.gameObject.GetComponent<HP>().damageDealt;
        }
    }
    #endregion

}

/*[CustomEditor(typeof(HP))]
public class HPEditor : Editor
{
    SerializedProperty isPlayer;
    SerializedProperty healthGradient;
    SerializedProperty healthText;
    SerializedProperty healthBar;
    SerializedProperty healthBarFill;
    SerializedProperty currentHp;
    SerializedProperty maxHp;
    SerializedProperty damageTaken;
    SerializedProperty damageDealt;

    void OnEnable()
    {
        isPlayer = serializedObject.FindProperty("isPlayer");
        healthGradient = serializedObject.FindProperty("healthGradient");
        healthText = serializedObject.FindProperty("healthText");
        healthBar = serializedObject.FindProperty("healthBar");
        healthBarFill = serializedObject.FindProperty("healthBarFill");
        currentHp = serializedObject.FindProperty("currentHp");
        maxHp = serializedObject.FindProperty("maxHp");
        damageTaken = serializedObject.FindProperty("damageTaken");
        damageDealt = serializedObject.FindProperty("damageDealt");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(isPlayer);
        EditorGUILayout.PropertyField(damageDealt);

        if (isPlayer.boolValue)
        {
            EditorGUILayout.PropertyField(healthGradient);
            EditorGUILayout.PropertyField(healthText);
            EditorGUILayout.PropertyField(healthBar);
            EditorGUILayout.PropertyField(healthBarFill);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(currentHp);
            EditorGUILayout.PropertyField(maxHp);
            EditorGUILayout.PropertyField(damageTaken);
        }

        serializedObject.ApplyModifiedProperties();
    }
}*/
