using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField] ParticleSystem boolet;

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            boolet.Play();
        }
    }
}
