using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour, ICustomMessageTarget
{
    [SerializeField] private float objectWidth;
    [SerializeField] public Texture icon;
    [SerializeField] private int woodReq = 30;
    [SerializeField] private int stoneReq = 30;

    private int invalidPlacement = 0;
    private bool movingObject = false;
    private bool sizeDynamic = false;
    private bool resizeBox = false;
    private Camera rayCamera;
    private float widthOffset;

    private bool finished = false;
    //private GameObject boundingBox = null;


    // Use this for initialization
    void Start()
    {
        widthOffset = objectWidth / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingObject)
            MovingBuilding();
        else if (sizeDynamic)
            SizingDynamicObject();
        else if (!finished && tag != "Stockpile")
            Construction();
    }

    private void Construction()
    {
        if (woodReq <= 0 && stoneReq <= 0)
        {
            finished = true;
            GetComponent<Animator>().SetBool("finished", true);
        }
    }

    public int DepositResource(string type, int amount)
    {
        if (type == "Wood")
        {
            woodReq -= amount;
            if (woodReq < 0)
            {
                return -woodReq;
            }
        }
        else
        {
            stoneReq -= amount;
            if (stoneReq < 0)
            {
                return -stoneReq;
            }
        }
        return 0;
    }

    public bool NeedWood()
    {
        if (woodReq > 0)
            return true;
        else
            return false;
    }

    public bool NeedStone()
    {
        if (stoneReq > 0)
            return true;
        else
            return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        invalidPlacement++;
    }

    private void OnTriggerExit(Collider other)
    {
        invalidPlacement--;
    }

    public void PlacingBuilding(Camera mainCamera)
    {
        movingObject = true;
        rayCamera = mainCamera;
    }

    public void PlacingDynamic(Camera mainCamera)
    { 
        sizeDynamic = true;
        rayCamera = mainCamera;
        

    }

    private void MovingBuilding()
    {
        RaycastHit hit;
        Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(Mathf.Round(hit.point.x) + widthOffset, transform.position.y, Mathf.Round(hit.point.z) + widthOffset);

        }

        if (Input.GetMouseButtonDown(0) && (invalidPlacement <= 0))
            movingObject = false;
        else if (Input.GetKeyDown("r") && gameObject.name != "Stockpile")
        {
            transform.Rotate(0.0f, 90.0f, 0.0f);
        }
        else if (Input.GetMouseButtonDown(1))
            Destroy(gameObject);
    }

    private void SizingDynamicObject()
    {
        RaycastHit hit;
        Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (!resizeBox)
            {
                transform.position = new Vector3(Mathf.Round(hit.point.x) + widthOffset, transform.position.y, Mathf.Round(hit.point.z) + widthOffset);
            }
            else
            {
                // yep gotta finish this out

            }

            if (Input.GetMouseButtonDown(0) && (invalidPlacement <= 0))
            {
                if (!resizeBox)
                    resizeBox = true;
                else
                {
                    ExecuteEvents.Execute<JCustomMessageTarget>(gameObject, null, (x, y) => x.SizeStockpile(transform.InverseTransformPoint(hit.point)));
                    resizeBox = false;
                    sizeDynamic = false;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Destroy(gameObject);
                sizeDynamic = false;
                resizeBox = false;
            }
        }
    }
}

public interface ICustomMessageTarget : IEventSystemHandler
{
    void PlacingBuilding(Camera mainCamera);
    void PlacingDynamic(Camera mainCamera);
}
