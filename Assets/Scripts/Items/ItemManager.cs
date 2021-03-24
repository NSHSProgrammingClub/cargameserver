using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    [Header("Inventory and Slots")]

    private Item currentItem;

    private int currentSlot;

    [SerializeField] private Item[] items = new Item[6];

    private bool switchOnPickup = false;

    int itemCount;


    [Header("Instantiation Locations")]
    [SerializeField] private Transform viewmodelPrefabLocation;

    //[SerializeField] private Transform playermodelPrefabLocation;


    [Header("Graphics")]
    //[SerializeField] private WeaponGraphics viewmodelGraphics;

    //[SerializeField] private WeaponGraphics playermodelGraphics;


    [Header("IK Rig Transforms")]
    //[SerializeField] private Transform rightHandGrip;
    //[SerializeField] private Transform leftHandGrip;




    private PlayerMovement controller;

    //[Header("Animation")]
    //[SerializeField] private Animator animator;
    //[SerializeField] private AnimatorOverrideController animatorOverride;


    //[Header("IK Rig Targets")]
    //[SerializeField] private Transform rightHandTarget;
    //[SerializeField] private Transform leftHandTarget;
    //[SerializeField] private Transform rightElbowHint;
    //[SerializeField] private Transform leftElbowHint;


    #region Item Slot References

    //[SerializeField] private GameObject slot1;
    //[SerializeField] private GameObject slot2;
    //[SerializeField] private GameObject slot3;
    //[SerializeField] private GameObject slot4;
    //[SerializeField] private GameObject slot5;
    //[SerializeField] private GameObject slot6;


    [Header("Item Slots")]
    //[SerializeField] private GameObject[] playermodelSlots;
    [SerializeField] private GameObject[] viewmodelSlots;


    #endregion



    //private Item secondary;


    // add others later

    void Start()
    {
        controller = GetComponent<PlayerMovement>();
        foreach (Item item in items)
        {
            if (item != null)
                currentSlot = item.slot; print("found highest item: " + item.itemName); break;
        }
        print("currentSlot: " + currentSlot);
        SwitchToSlot(currentSlot);
    }




    #region Get private properties
    public Item GetCurrentItem()
    {
        return currentItem;
    }

    //public WeaponGraphics GetCurrentViewmodelGraphics()
    //{
    //    return viewmodelGraphics;
    //}

    //public WeaponGraphics GetCurrentPlayermodelGraphics()
    //{
    //    return playermodelGraphics;
    //}
    #endregion


    private void Equip(Item item, bool firstTime)
    {
        currentItem = item;


        GameObject viewmodel;
        //GameObject playermodel;

        // spawning items

        //player model(external)
        if (firstTime)
        {
            //playermodel = Instantiate
            //(            
            //    playermodelPrefabLocation.position,                   // position
            //    playermodelPrefabLocation.rotation,                   // rotation
            //    playermodelSlots[item.slot - 1].transform             // parent
            //);


            // viewmodel
            viewmodel = Instantiate
            (
                currentItem.viewmodelPrefab,                          // model
                viewmodelPrefabLocation.position,                     // position
                viewmodelPrefabLocation.rotation,                     // rotation
                viewmodelSlots[item.slot - 1].transform               // parent
            );

        }
        else
        {
            viewmodel = viewmodelSlots[item.slot - 1].transform.GetChild(0).gameObject;
            //playermodel = playermodelSlots[item.slot - 1].transform.GetChild(0).gameObject;
        }


        //viewmodelGraphics = viewmodel.GetComponent<WeaponGraphics>();
        //playermodelGraphics = playermodel.GetComponent<WeaponGraphics>();

        //if (currentItem is Weapon)
        //{
        //    viewmodelGraphics.recoilAmount = (currentItem as Weapon).recoilAmount;
        //    playermodelGraphics.recoilAmount = (currentItem as Weapon).recoilAmount;

        //    AlignTransforms();
        //}

        Utils.SetLayer(viewmodel, viewmodel.gameObject.transform.parent.gameObject.layer);
        //Utils.SetLayer(playermodel, playermodel.transform.parent.gameObject.layer);

    }


    public void SwitchToSlot(int slot)
    {
        if (items[slot - 1])
        {
            //playermodelSlots[currentItem.slot - 1].SetActive(false);
            //print(currentItem.itemName);
            //viewmodelSlots[currentItem.slot - 1].SetActive(false);

            // first time equipping
            if (viewmodelSlots[slot - 1].transform.childCount <= 0)
            {
                if (currentItem)
                {
                    //playermodelSlots[currentItem.slot - 1].SetActive(false);
                    viewmodelSlots[currentItem.slot - 1].SetActive(false);
                }
                Equip(items[slot - 1], true);
            }
            // not first time equipping
            else
            {
                //playermodelSlots[currentItem.slot - 1].SetActive(false);
                viewmodelSlots[currentItem.slot - 1].SetActive(false);
                Equip(items[slot - 1], false);
            }

            currentSlot = slot;
            //playermodelSlots[slot - 1].SetActive(true);
            viewmodelSlots[slot - 1].SetActive(true);
        }

    }

    public void Pickup(Item item)
    {
        if (itemCount >= 6)
            return;

        if (switchOnPickup)
        {
            items[item.slot - 1] = item;
        }

        itemCount++;
    }

    public void Drop(int slot)
    {
        if (items[slot - 1])
        {
            items[slot - 1] = null;
            SwitchToSlot(slot + 1);
        }
    }


    public void Unequip(Item item)
    {
        //playermodelSlots[item.slot - 1].SetActive(false);
        viewmodelSlots[item.slot - 1].SetActive(false);
    }

    private void GetInput()
    {
        // set up weapon slots in editor
        if (Input.GetButtonDown("Slot1"))
            SwitchToSlot(1);
        if (Input.GetButtonDown("Slot2"))
            SwitchToSlot(2);
        if (Input.GetButtonDown("Slot3"))
            SwitchToSlot(3);
        if (Input.GetButtonDown("Slot4"))
            SwitchToSlot(4);
        if (Input.GetButtonDown("Slot5"))
            SwitchToSlot(5);
        if (Input.GetButtonDown("Slot6"))
            SwitchToSlot(6);
    }

    // ugly code but it works and not too expensive
    //private void AlignTransforms()
    //{
    //    rightHandTarget.localPosition = (currentItem as Weapon).graphics.rightHandTarget.transform.localPosition;
    //    rightHandTarget.localRotation = (currentItem as Weapon).graphics.rightHandTarget.transform.localRotation;

    //    leftHandTarget.localPosition = (currentItem as Weapon).graphics.leftHandTarget.transform.localPosition;
    //    leftHandTarget.localRotation = (currentItem as Weapon).graphics.leftHandTarget.transform.localRotation;

    //    rightElbowHint.localPosition = (currentItem as Weapon).graphics.rightElbowHint.transform.localPosition;
    //    rightElbowHint.localRotation = (currentItem as Weapon).graphics.rightElbowHint.transform.localRotation;

    //    leftElbowHint.localPosition = (currentItem as Weapon).graphics.leftElbowHint.transform.localPosition;
    //    leftElbowHint.localRotation = (currentItem as Weapon).graphics.leftElbowHint.transform.localRotation;
    //}

    private void Update()
    {
        GetInput();
    }
}
