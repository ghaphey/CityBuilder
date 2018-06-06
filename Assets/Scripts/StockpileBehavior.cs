using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileBehavior : MonoBehaviour {
    [SerializeField] private int capacity = 100;
    private StockSlot[] currStock = new StockSlot[4];

    // want to designate 4 slots w/ capacity and keep track of what is here
	// Use this for initialization
	void Start () {
		foreach (StockSlot slot in currStock)
        {
            slot.number = 0;
            slot.rTag = null;
        }
	}
	
	// Update is called once per frame
	
    public int DepositItem (string itemTag, int amount)
    {
        int numLeft = amount;
        foreach (StockSlot slot in currStock)
        {
            if (numLeft <= 0)
                return 0;
            if (slot.rTag == itemTag && slot.number < capacity)
            {
                slot.number += amount;
                if (slot.number > capacity)
                {
                    numLeft = capacity - slot.number;
                }
                else
                    return 0;

            }
        }
        return numLeft;
    }

    public int TakeItem (string itemTag, int amount)
    {
        int numLeft = 0;
        foreach (StockSlot slot in currStock)
        {
            if (numLeft == amount)
                return numLeft;
            if (itemTag == slot.rTag)
            {
                numLeft = 
                    // take the number we need, then adjust both to fix
            }
        }
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
    }
}
