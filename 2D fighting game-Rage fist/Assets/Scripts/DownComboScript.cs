using UnityEngine;

public class DownComboScript : MonoBehaviour
{
    [SerializeField] float comboTimeLimit = 1.0f; // Time limit to perform a combo
    float lastComboTime = 0; // Time of the last combo input

    [SerializeField] float timeBetweenHits = 0.5f; // Time between 2 inputs
    [SerializeField] float currentTimeBetweenCombos = 0f;

    private int comboCount = 0; // Current combo count


    public Animator playerAnimator;
    void Update()
    {
        // check if enough time passed since last combo
        if (Time.time - lastComboTime > comboTimeLimit)
        {
            // reset if too much time has passed
            comboCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.K) && currentTimeBetweenCombos <= Time.time)
        {
            if(Input.GetKey(KeyCode.S))
            {
                // set on next combo
                comboCount++;

                currentTimeBetweenCombos = Time.time + timeBetweenHits;

                switch (comboCount)
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
                        comboCount = 0;
                        // Do same for more combos
                        break;
                }
            }
            

            // update the time of the last combo input
            lastComboTime = Time.time;
        }
    }
}
