using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileSlotBehavior : MonoBehaviour
{
    [SerializeField] private int capacity = 50;

    private class Stock
    {
        public string rTag;
        public int number;
        public GameObject item;
    }

    private Stock currItem;

    private void Start()
    {
        
    }

    public void InitStock()
    {
        currItem = new Stock { rTag = null, number = 0, item = null };
    }

    public void UpdateItem()
    {
        if (currItem.number <= 0)
        {
            currItem.number = 0;
            currItem.rTag = null;
            if (currItem.item != null)
                Destroy(currItem.item);
            currItem.item = null;
        }
        else
            currItem.item.GetComponentInChildren<TextMesh>().text = currItem.number.ToString();
    }



    public void CreateItem(GameObject item)
    {
        print(currItem.number);
        currItem.rTag = item.name;
        currItem.item = Instantiate(item, gameObject.transform, false);
        currItem.item.GetComponentInChildren<TextMesh>().text = currItem.number.ToString();
    }

    // simple check for stockpilebehavior to call
    public bool ItemCheck(string name)
    {
        if (name == currItem.rTag)
            return true;
        else
            return false;
    }

    public bool CapacityLeft()
    {
        if (currItem.number != capacity)
            return true;
        else
            return false;
    }

    // deposit item, MUST CHECK IF ITEM MATCHES FIRST, diff function
    // returns the number of remaining items unsuccessfully deposited
    public int DepositItem(int amount)
    {
        // if an item doesn't currently exist, create it
        // should check for matching items before calling this function
        int numLeft = amount;
        // if we are already over capacity, return
        if (currItem.number >= capacity)
            return numLeft;
        else
        {
            // add requested deposit
            currItem.number += numLeft;
            if (currItem.number > capacity)
            {
                // if we are over capacity, adjust numleft + current item number then return numleft
                numLeft = currItem.number - capacity;
                currItem.number -= numLeft;
                return numLeft;
            }
            else // if we didn't exceed capacity that means the number was completely deposited, return 0
                return 0;
        }
    }

    // takes item from this stock, returns number successfully taken
    // should have checked to ensure this stock contains the desired item first before
    // calling function
    public int TakeItem(int amount)
    {
        int numleft = amount;

        // subtract the amount from the current stock number
        currItem.number -= amount;
        if (currItem.number >= 0) // if its above zero, we're good no adjustments needed
            return numleft;
        else
        {
            // if below zero, we need to adjust numleft and set current item num to zero
            numleft += currItem.number;
            currItem.number = 0;
            // return the amount unsuccessfully deposited
            return numleft;
        }
    }

}
