using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour, ICustomMessageTarget
{
    [SerializeField] private float objectWidth;
    [SerializeField] public Texture icon;
    [SerializeField] private float dynamicSizeMax = 1.0f;

    private int invalidPlacement = 0;
    private bool movingObject = false;
    private bool sizeDynamic = false;
    private bool resizeBox = false;
    private Camera rayCamera;
    private float widthOffset;
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
        /*
        boundingBox = GameObject.FindWithTag("BoundingBox");
        boundingBox.transform.SetParent(gameObject.transform);
        boundingBox.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
        /// finish fixing dynamic placement
        */

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
                // need to figure out how to SHOW that this is being placed

                //float xDiff = hit.point.x - gameObject.transform.position.x;
                //float zDiff = hit.point.z - gameObject.transform.position.z

                //transform.localScale = new Vector3(Mathf.Clamp(xDiff / 10, -dynamicSizeMax, dynamicSizeMax), 0.1f, 0.1f);
                //transform.localPosition = new Vector3(xDiff, 0.001f, 0.5f/*Mathf.Round(zDiff) / 2*/);
            }
            // yep gotta finish this out

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
                /*
                boundingBox.transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
                boundingBox.transform.localPosition = new Vector3(0.5f, -10.0f, 0.5f); */
            }
        }

        /*
        print(invalidPlacement);
        RaycastHit hit;
        Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (!resizeBox)
            {
                boundingBox.transform.position = new Vector3(Mathf.Round(hit.point.x) + widthOffset, transform.position.y, Mathf.Round(hit.point.z) + widthOffset);
                transform.position = new Vector3(Mathf.Round(hit.point.x) + widthOffset, transform.position.y, Mathf.Round(hit.point.z) + widthOffset);
            }
            else
            {
                float xDiff = hit.point.x - gameObject.transform.position.x;
                float zDiff = hit.point.z - gameObject.transform.position.z;
                boundingBox.transform.localScale = new Vector3(Mathf.Round(xDiff), 0.5f, Mathf.Round(zDiff));
                boundingBox.transform.localPosition = new Vector3(Mathf.Round(xDiff) / 2, 0.5f, Mathf.Round(zDiff) / 2);
            }
            // yep gotta finish this out
        }
        if (Input.GetMouseButtonDown(0) && (invalidPlacement <= 0))
        {
            if  (resizeBox == false)
                resizeBox = true;
            else
                ExecuteEvents.Execute<JCustomMessageTarget>(gameObject, null, (x, y) => x.SizeStockpile(rayCamera));
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
            sizeDynamic = false;
            resizeBox = false;
            boundingBox.transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            boundingBox.transform.localPosition = new Vector3(0.5f, -10.0f, 0.5f);
        }
        */
    }
}

public interface ICustomMessageTarget : IEventSystemHandler
{
    void PlacingBuilding(Camera mainCamera);
    void PlacingDynamic(Camera mainCamera);
}
