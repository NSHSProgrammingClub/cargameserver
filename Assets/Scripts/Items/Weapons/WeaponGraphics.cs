using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGraphics : MonoBehaviour
{

    [Header("Effects")]

    [SerializeField] private ParticleSystem muzzleFlash;

    
    
    public void OnFire()
    {
        muzzleFlash.Play();
    }

}
