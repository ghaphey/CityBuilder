using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class StockpileBehavior : MonoBehaviour, JCustomMessageTarget
{

    private bool sizingStockpile = false;
    private Camera RayCamera;

    private void Start()
    {
    }

    private void Update()
    {
        if (sizingStockpile)
            SizingStockpile();
    }

    private void SizingStockpile()
    {
    } // must re-implement most due to changes in how placing objects work

    public void SizeStockpile(Camera rayCamera)
    {
    }


    public int DepositItem(int amount, GameObject newItem)
    {
        return 0;
    }

    public int TakeItem(int amount)
    {
        return 0;
    }

    public bool SpaceAvailable(string itemType)
    {
        return false;
    }
}

public interface JCustomMessageTarget : IEventSystemHandler
{
    void SizeStockpile(Camera rayCamera);

}