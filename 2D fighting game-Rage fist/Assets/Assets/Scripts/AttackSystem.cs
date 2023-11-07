using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This code is for the functions of attacks, we will call them in the PlayerController script.
/// Combos are normal attacks, don't get confused.
/// </summary>

public class AttackSystem : MonoBehaviour
{
    public Animator playerAnimator;

    #region Down Combo
    [SerializeField] float downComboTimeLimit = 1.0f; // Time limit to perform a combo
    private float downCombolastComboTime = 0; // Time of the last combo input

    [SerializeField] float downComboTimeBetweenHits = 0.5f; // Time between 2 inputs
    [SerializeField] float timeBetweenDownCombos = 0f;

    private int downComboCount = 0; // Current combo count
    #endregion

    #region Up Combo
    #endregion

    // Down combo
    public void downCombo()
    {
        // check if enough time passed since last combo
        if (Time.time - downCombolastComboTime > downComboTimeLimit)
        {
            // reset if too much time has passed
            downComboCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.K) && timeBetweenDownCombos <= Time.time)
        {
            if (Input.GetKey(KeyCode.S))
            {
                // set on next downCombo
                downComboCount++;

                timeBetweenDownCombos = Time.time + downComboTimeBetweenHits;

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
            downCombolastComboTime = Time.time;
        }
    }
}
