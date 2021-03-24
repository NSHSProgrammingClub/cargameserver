using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    /*[SerializeField] */private ItemManager itemManager;
    private Weapon weapon;

    [SerializeField] private Camera playerCamera;

    [SerializeField] private LayerMask targets;


    private void Start()
    {
        itemManager = GetComponent<ItemManager>();

        if (itemManager.GetCurrentItem() is Weapon)
            weapon = itemManager.GetCurrentItem() as Weapon;
    }


    private void Fire()
    {

        RaycastHit hits;

        bool hasHit = Physics.Raycast(playerCamera.gameObject.transform.position, playerCamera.gameObject.transform.forward, out hits, weapon.range, targets);

        Debug.DrawRay(playerCamera.gameObject.transform.position, playerCamera.gameObject.transform.forward, new Color(255, 0, 0), weapon.range);

        weapon.graphics.OnFire();

        if (hasHit)
        {
            // means that the bullet hit something
            Debug.Log("Hit target " + hits.collider.name);
        }

        print("player fired");
    }

    private void GetInput()
    {

        if (Input.GetButtonDown("Firemode"))
        {
            if (Array.IndexOf(weapon.firemodeType, weapon.currentFireModeType) + 1 <= weapon.firemodeType.Length - 1)
            {
                weapon.currentFireModeType = weapon.firemodeType[Array.IndexOf(weapon.firemodeType, weapon.currentFireModeType) + 1];
                Debug.Log(Array.IndexOf(weapon.firemodeType, weapon.currentFireModeType) + 1);
                Debug.Log("Switched to " + weapon.currentFireModeType.ToString());
            }
            else
            {
                weapon.currentFireModeType = weapon.firemodeType[0];
                Debug.Log("Switched to " + weapon.currentFireModeType.ToString());
            }

        }

        if (weapon.currentFireModeType == FiremodeType.automatic)
        {
            // automatic firing
            if (Input.GetButtonDown("Fire1"))
                InvokeRepeating("Fire", 0f, 60f / weapon.firerate);
            else if (Input.GetButtonUp("Fire1"))
                CancelInvoke("Fire");
        }
        else
        {
            // semiautomatic
            if (Input.GetButtonDown("Fire1"))
                Fire();
        }
    }

    private void Update()
    {
        Item item = itemManager.GetCurrentItem();
        if (item is Weapon)
            weapon = item as Weapon;
        GetInput();
    }
}
