using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This code is for the functions of attacks, we will call them in the PlayerController script.
/// There are 2 types of attacks, combos and normal ones.
/// </summary>

public class AttackSystem : MonoBehaviour
{
    public Animator playerAnimator;

    // Normal attack variables
    [Header("Normal attack")]
    #region Normal attack variables
    [SerializeField] float attackTimeLimit = 1.0f;
    private float lastAttackTime = 0;

    [SerializeField] float timeBetweenAttackHits = 0.5f;
    [SerializeField] float currentTimeBetweenAttack = 0f;

    private int attackCount = 0;
    #endregion

    // Down combo variables
    [Header("Down combo")]
    #region Down combo
    [SerializeField] float downComboTimeLimit = 1.0f; // Time limit to perform a combo
    private float downComboLastComboTime = 0; // Time of the last combo input

    [SerializeField] float downComboTimeBetweenHits = 0.5f; // Time between 2 inputs
    [SerializeField] float currentTimeBetweenDownCombos = 0f;

    private int downComboCount = 0; // Current combo count
    #endregion

    // Up combo variables
    [Header("Up combo")]
    #region Up combo variables
    [SerializeField] float upComboTimeLimit = 1.0f; // Time limit to perform a combo
    float lastUpComboTime = 0; // Time of the last combo input

    [SerializeField] float timeBetweenUpComboHits = 0.5f; // Time between 2 inputs
    [SerializeField] float currentTimeBetweenUpCombos = 0f;

    private int upComboCount = 0; // Current combo count
    #endregion

    // Side combo variables
    [Header("Side combo")]
    #region Side combo variables
    [SerializeField] float sideComboTimeLimit = 1.0f; // Time limit to perform a combo
    private float lastSideComboTime = 0; // Time of the last combo input

    [SerializeField] float timeBetweenSideComboHits = 0.5f; // Time between 2 inputs
    [SerializeField] float currentTimeBetweenSideCombos = 0f;

    private int sideComboCount = 0; // Current combo count
    #endregion

    // Attacks
    #region Normal attacks and combos
    // Normal attacks
    public void NormalAttacks()
    {
        // check if enough time passed since last combo
        if (Time.time - lastAttackTime > attackTimeLimit)
        {
            // reset if too much time has passed
            attackCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.K) && currentTimeBetweenAttack <= Time.time && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A))
        {
            // set on next combo
            attackCount++;

            currentTimeBetweenAttack = Time.time + currentTimeBetweenAttack;

            switch (attackCount)
            {
                case 1:
                    Debug.Log("1");
                    //playerAnimator.SetTrigger("Attack1");
                    break;
                case 2:
                    //playerAnimator.SetTrigger("Attack2");
                    Debug.Log("2");
                    break;
                case 3:
                    //playerAnimator.SetTrigger("Attack3");
                    Debug.Log("3");
                    break;
                default:
                    attackCount = 0;
                    // Do same for more combos
                    break;
            }

            // update the time of the last combo input
            lastAttackTime = Time.time;
        }
    }

    // Down combo
    public void DownCombo()
    {
        // check if enough time passed since last combo
        if (Time.time - downComboLastComboTime > downComboTimeLimit)
        {
            // reset if too much time has passed
            downComboCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (currentTimeBetweenDownCombos <= Time.time)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    // set on next combo
                    downComboCount++;

                    currentTimeBetweenDownCombos = Time.time + downComboTimeBetweenHits;

                    switch (downComboCount)
                    {
                        case 1:
                            Debug.Log("D1");
                            //playerAnimator.SetTrigger("Attack1");
                            break;
                        case 2:
                            //playerAnimator.SetTrigger("Attack2");
                            Debug.Log("D2");
                            break;
                        case 3:
                            //playerAnimator.SetTrigger("Attack3");
                            Debug.Log("D3");
                            break;
                        default:
                            downComboCount = 0;
                            // Do same for more combos
                            break;
                    }
                }

                // update the time of the last combo input
                downComboLastComboTime = Time.time;
            }
        }
    }

    // Up combo
    public void UpCombo()
    {
        // check if enough time passed since last combo
        if (Time.time - lastUpComboTime > upComboTimeLimit)
        {
            // reset if too much time has passed
            upComboCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (currentTimeBetweenUpCombos <= Time.time)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    // set on next combo
                    upComboCount++;

                    currentTimeBetweenUpCombos = Time.time + timeBetweenUpComboHits;

                    switch (upComboCount)
                    {
                        case 1:
                            Debug.Log("U1");
                            //playerAnimator.SetTrigger("Attack1");
                            break;
                        case 2:
                            //playerAnimator.SetTrigger("Attack2");
                            Debug.Log("U2");
                            break;
                        case 3:
                            //playerAnimator.SetTrigger("Attack3");
                            Debug.Log("U3");
                            break;
                        default:
                            upComboCount = 0;
                            // Do same for more combos
                            break;
                    }
                }

                // update the time of the last combo input
                lastUpComboTime = Time.time;
            }
        }
    }

    // Side combo
    public void SideCombo() 
    {
        // check if enough time passed since last combo
        if (Time.time - lastSideComboTime > sideComboTimeLimit)
        {
            // reset if too much time has passed
            sideComboCount = 0;
        }

        if ( Input.GetKeyDown(KeyCode.K)) 
        {
            if (currentTimeBetweenSideCombos <= Time.time)
            {
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {
                    // set on next combo
                    sideComboCount++;

                    currentTimeBetweenSideCombos = Time.time + timeBetweenSideComboHits;

                    switch (sideComboCount)
                    {
                        case 1:
                            Debug.Log("S1");
                            //playerAnimator.SetTrigger("Attack1");
                            break;
                        case 2:
                            //playerAnimator.SetTrigger("Attack2");
                            Debug.Log("S2");
                            break;
                        case 3:
                            //playerAnimator.SetTrigger("Attack3");
                            Debug.Log("S3");
                            break;
                        default:
                            sideComboCount = 0;
                            // Do same for more combos
                            break;
                    }
                }

                // update the time of the last combo input
                lastSideComboTime = Time.time;
            }
        }
    }
    #endregion

    private void Update()
    {
        NormalAttacks();

        DownCombo();
        UpCombo();
        SideCombo();
    }
}
