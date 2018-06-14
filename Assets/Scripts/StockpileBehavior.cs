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
        ////??????? resizing a 2d array is intensive so original idea probably won't work effectively
        //probably would be better to create a temp mesh box that expands with the cursor to show the area that the
        // new stockpile will occupy. once the mouse is clicked to indicate position THEN it spawns all of the stockpile thigns
        // probably should do the same thing for housees and other buildings
        RaycastHit hit;
        Ray ray = RayCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            float xDiff, zDiff;
            xDiff = hit.point.x - gameObject.transform.position.x;
            zDiff = gameObject.transform.position.z - hit.point.z;
            if (xDiff > 0.0f)
            {
                for (int i = 0; i < Mathf.Round(xDiff); i++)
                {
                    print(i);
                    if (stockpileSlots[0, i] == null)
                    {
                        stockpileSlots[0, i] = Instantiate(stockpileSlots[0, 0],gameObject.transform);
                        stockpileSlots[0, i].transform.localPosition = new Vector3(Mathf.Round(xDiff) + 0.5f, 0.001f, 0.5f);

                    }
                    else
                        stockpileSlots[0, i].SetActive(true);
                }
            }
        }
    }

    public void SizeStockpile(Camera rayCamera)
    {
        RayCamera = rayCamera;
        sizingStockpile = true;
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