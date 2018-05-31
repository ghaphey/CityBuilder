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

    [Header("Camera")]
    [SerializeField] float cameraOffset = 20;
    
    // Use this for initialization
    void Start ()
    {
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
    }

    void PanCamera ()
    {
        float xAxisMove = Input.GetAxis("Horizontal");
        float zAxisMove = Input.GetAxis("Vertical");

        float xOffset = xAxisMove * panSpeed * Time.deltaTime;
        float zOffset = zAxisMove * panSpeed * Time.deltaTime;

        // perform the clamp calc to keep within boundary
        if ((transform.position.x + xOffset) > xBounds || (transform.position.x + xOffset) < -xBounds)
            xOffset = 0.0f;
        if ((transform.position.z + zOffset) > zBounds || (transform.position.z + zOffset) < -zBounds)
            zOffset = 0.0f;

        transform.Translate(new Vector3(xOffset, 0.0f, zOffset));
    }

    void ZoomCamera ()
    {
        float yPos = 0;
        // based on "player" object position to rotate around point instead of rotate camera
        float yAxisMove = Input.GetAxis("Mouse ScrollWheel");
        float yOffset = yAxisMove * zoomSpeed * Time.deltaTime;
        float yRawPos = transform.localPosition.y + yOffset;
        yPos = Mathf.Clamp(yRawPos, yBoundsMin - cameraOffset, yBoundsMax - cameraOffset);

        transform.localPosition = new Vector3(transform.localPosition.x, yPos, transform.localPosition.z);
    }

    private void RotateCamera()
    {
        float rAxisMove = Input.GetAxis("Rotation");
        Vector3 cameraRot = transform.eulerAngles;

        cameraRot.y += rAxisMove * rotateSpeed * Time.deltaTime;

        transform.eulerAngles = cameraRot;
    }

}
