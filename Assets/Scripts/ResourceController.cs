using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private int resourceValue = 100;
    [SerializeField] private float workTime = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (resourceValue <= 0)
            Destroy(gameObject);
	}

    public int TakeResource( int numTaken )
    {
        resourceValue -= numTaken;
        if (resourceValue < 0)
            return numTaken - resourceValue;
        else
            return numTaken;
    }

    public float GetWorkTime()
    {
        return workTime;
    }
}
