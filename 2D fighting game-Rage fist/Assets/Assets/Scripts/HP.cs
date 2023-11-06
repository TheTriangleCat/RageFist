using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public float currentHp = 0;
    public float maxHp = 10;
    public float damageTaken = 0;
    public float shieldDmgReducer = 25;
    public float damageDealt = 1;

    void Start()
    {
        currentHp = maxHp;
    }

    
    void Update()
    {
        if (currentHp <=0)
        {
            Debug.Log("You died");
        }

        if(Input.GetKey(KeyCode.F))
        {
            damageTaken = damageTaken * 100 / (100 - shieldDmgReducer);
            Debug.Log(damageTaken);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "damage")
        {
            damageTaken = collision.gameObject.GetComponent<HP>().damageDealt;
        }
    }

}
