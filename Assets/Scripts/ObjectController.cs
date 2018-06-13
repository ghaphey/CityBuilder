using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour, ICustomMessageTarget
{
    [SerializeField] private float objectWidth;
    [SerializeField] public Texture icon;
    
    private int invalidPlacement = 0;
    private bool movingObject = false;
    private Camera rayCamera;
    private float widthOffset;


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

    private void MovingBuilding ()
    {
        RaycastHit hit;
        Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
        movingObject = true;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(Mathf.Round(hit.point.x) + widthOffset, transform.position.y, Mathf.Round(hit.point.z) + widthOffset);

        }

        if (Input.GetMouseButtonDown(0) && (invalidPlacement <= 0))
            movingObject = false;
        else if (Input.GetKeyDown("r"))
        {
            transform.Rotate(0.0f, 90.0f, 0.0f);
        }
        else if (Input.GetMouseButtonDown(1))
            Destroy(gameObject);
    }

}

public interface ICustomMessageTarget : IEventSystemHandler
{
    void PlacingBuilding(Camera mainCamera);
}
