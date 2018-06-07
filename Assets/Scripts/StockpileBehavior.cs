using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileBehavior : MonoBehaviour {
    [SerializeField] private int capacity = 100;
    [SerializeField] private GameObject wood;
    [SerializeField] private GameObject stone;
    private StockSlot[] currStock = new StockSlot[4];

    // want to designate 4 slots w/ capacity and keep track of what is here
	// Use this for initialization
	void Start () {
		foreach (StockSlot slot in currStock)
        {
            slot.number = 0;
            slot.rTag = null;
            slot.item = null;
        }
	}

    // Update is called once per frame
    private void Update()
    {
        int i = 0;
        foreach (StockSlot slot in currStock)
        {
            i++;
            if (slot.number == 0 && slot.item != null)
            {
                Destroy(slot.item);
                slot.item = null;
            }
            else if (slot.number > 0 && slot.item == null)
            {
                switch(slot.rTag)
                {
                    case "Wood":
                        slot.item = createItemAtSlot(i, wood);
                        break;
                    case "Stone":
                        slot.item = createItemAtSlot(i, stone);
                        break;
                    default:
                        print("Unknown item");
                        break;

                }
            }
        }
    }

    private GameObject createItemAtSlot( int index, GameObject item)
    {
        float x, z;
        switch (index)
        {
            case 0:
                x = 2.0f;
                z = 2.0f;
                break;
            case 1:
                x = 2.0f;
                z = -2.0f;
                break;
            case 2:
                x = -2.0f;
                z = 2.0f;
                break;
            case 3:
                x = -2.0f;
                z = -2.0f;
                break;
            default:
                x = 0.0f;
                z = 0.0f;
                break;
        }
        return Instantiate(item, new Vector3(x, 0.2f, z), Quaternion.identity, gameObject.transform);
    }

    public int DepositItem (string itemTag, int amount)
    {
        int numLeft = amount;
        foreach (StockSlot slot in currStock)
        {
            if (numLeft <= 0)
                return 0;
            if ( (slot.rTag == itemTag || slot.rTag == null) && slot.number < capacity)
            {
                slot.rTag = itemTag;
                slot.number += numLeft;
                if (slot.number > capacity)
                {
                    numLeft = capacity - slot.number;
                    slot.number -= numLeft;
                }
                else
                    return 0;

            }
        }
        return numLeft;
    }

    public int TakeItem (string itemTag, int amount)
    {
        int total = 0;
        int numLeft = amount;
        foreach (StockSlot slot in currStock)
        {
            if (itemTag == slot.rTag)
            {
                // if number left can take from a slot without emptying, return running total + number left
                if(slot.number - numLeft > 0)
                {
                    slot.number -= numLeft;
                    return numLeft + total;
                }
                // if number left empties a slot perfectly, must set slot as empty
                else if (slot.number - numLeft == 0)
                {
                    slot.number = 0;
                    slot.rTag = null;
                    return numLeft + total;
                }
                // if slot to negative, empty slot, reduce numleft and increase total
                else
                {
                    total += slot.number;
                    numLeft -= slot.number;
                    slot.number = 0;
                    slot.rTag = null;
                }

            }
        }
        // if we reach here, was not able to get enough resource for request, leave numleft only return running total
        return total;
    }

    public bool QueryItem (string itemTag)
    {
        foreach (StockSlot slot in currStock)
        {
            if (itemTag == slot.rTag)
                return true;
        }
        return false;
    }
    

    private class StockSlot
    {
        public string rTag;
        public int number;
        public GameObject item;
    }
}
