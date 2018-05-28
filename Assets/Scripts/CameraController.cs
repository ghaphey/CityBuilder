using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Boundaries")]
    [Tooltip("Width of Field")] [SerializeField] float xBounds = 10;
    [Tooltip("Width of Field")] [SerializeField] float zBounds = 10;
    [Tooltip("Zoom out max")] [SerializeField] float yBoundsMax = 30;
    [Tooltip("Zoom in max")] [SerializeField] float yBoundsMin = 5;

    [Header("Properties")]
    [SerializeField] float panSpeed = 1;
    [SerializeField] float zoomSpeed = 1;
    [SerializeField] float rotateSpeed = 1;

    private float xPos = 0;
    private float yPos = 0;
    private float zPos = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MakeMoves();
    }

    private void MakeMoves()
    {
        PanCamera();
        ZoomCamera();
        RotateCamera();
        transform.localPosition = new Vector3(xPos, yPos, zPos);
    }

    // does not pan based on camera position, its based on global for now, weird
    void PanCamera ()
    {
        float xAxisMove = Input.GetAxis("Horizontal");
        float zAxisMove = Input.GetAxis("Vertical");

        float xOffset = xAxisMove * panSpeed * Time.deltaTime;
        float zOffset = zAxisMove * panSpeed * Time.deltaTime;

        float xRawPos = transform.localPosition.x + xOffset;
        float zRawPos = transform.localPosition.z + zOffset;

        xPos = Mathf.Clamp(xRawPos, -xBounds, xBounds);
        zPos = Mathf.Clamp(zRawPos, -zBounds, zBounds);
    }

    // has desired behavior
    void ZoomCamera ()
    {
        float yAxisMove = Input.GetAxis("Mouse ScrollWheel");
        float yOffset = yAxisMove * zoomSpeed * Time.deltaTime;
        float yRawPos = transform.localPosition.y + yOffset;
        yPos = Mathf.Clamp(yRawPos, yBoundsMin, yBoundsMax);
    }

    // need to edit so it rotates around a center axis
    private void RotateCamera()
    {
        float rAxisMove = Input.GetAxis("Rotation");
        Vector3 cameraRot = transform.eulerAngles;

        cameraRot.y += rAxisMove * rotateSpeed * Time.deltaTime;

        transform.eulerAngles = cameraRot;
    }

}
