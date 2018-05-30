using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private int invalidPlacement = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //returns 0 if not colliding with any object
    public int GetPlacement()
    {
        return invalidPlacement;
    }

    private void OnTriggerEnter(Collider other)
    {
        invalidPlacement++;
    }

    private void OnTriggerExit(Collider other)
    {
        invalidPlacement--;
    }
}
