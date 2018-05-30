using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildElementUI : MonoBehaviour {

    [SerializeField] GameObject house;
    [SerializeField] Camera mainCamera;

    private bool movingObject;
    private Vector3 rayCastTarget;
    private GameObject newObject;
    private bool invalidPlace = false;

    private void Update()
    {
        if (movingObject)
            ObjectPlace();
    }


    public void HousePressed ()
    {
        newObject = Instantiate(house, new Vector3(0.0f, 0.45f, 0.0f), Quaternion.identity);
        movingObject = true;

      
    }

    

    void ObjectPlace ()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            newObject.transform.position = new Vector3(Mathf.Round(hit.point.x), newObject.transform.position.y, Mathf.Round(hit.point.z));

        }

        if (Input.GetMouseButtonDown(0) && !invalidPlace)
            movingObject = false;
        // MUST IMPLEMENT COLLISION DETECTION FOR BUILDING PLACEMENT
        else if (Input.GetKeyDown("r"))
        {
            newObject.transform.Rotate(0.0f, 90.0f, 0.0f);
        }
    }

}
