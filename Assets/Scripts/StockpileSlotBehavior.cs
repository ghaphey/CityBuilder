using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileSlotBehavior : MonoBehaviour
{
    [SerializeField] private int capacity = 50;

    private Stock currItem;
    void Start()
    {
        //initialize
        currItem = new Stock
        {
            number = 0,
            rTag = null,
            item = null
        };
    }

    // Update is called once per frame
    private void Update()
    {
        // just in case, < 0 reset stock
        if (currItem.number < 0)
        {
            currItem.number = 0;
            currItem.rTag = null;
        }
        // if the number is zero, and the item still exists, reset the stock
        else if (currItem.number == 0 && currItem.item != null)
        {
            Destroy(currItem.item);
            currItem.item = null;
            currItem.rTag = null;
        }
        // if the item exists and the number is > 0 we will update the text
        else if (currItem.item != null)
            currItem.item.GetComponentInChildren<TextMesh>().text = currItem.number.ToString();
    }

    // create passed item centered in the stockpile
    private void CreateItem(GameObject item)
    {
        currItem.item = item;
        currItem.rTag = item.name;
        GameObject newItem = Instantiate(item, gameObject.transform, false);
        newItem.transform.localPosition = new Vector3(0.0f, 01.5f, 0.0f);
    }

    // simple check for stockpilebehavior to call
    public bool ItemCheck(string name)
    {
        if (name == currItem.rTag)
            return true;
        else
            return false;
    }

    // deposit item, MUST CHECK IF ITEM MATCHES FIRST, diff function
    // returns the number of remaining items unsuccessfully deposited
    public int DepositItem(int amount, GameObject newItem)
    {
        // if an item doesn't currently exist, create it
        // should check for matching items before calling this function
        if (currItem.item == null)
            CreateItem(newItem);
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
                numLeft = capacity - currItem.number;
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

    private class Stock
    {
        public string rTag;
        public int number;
        public GameObject item;
    }
}
