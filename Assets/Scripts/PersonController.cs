using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonController : MonoBehaviour {

    private GameObject goal = null;

    private NavMeshAgent agent;
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        goal = FindClosestLocation("Gather");
        if (goal != null)
        {
            print("moving");
            agent.destination = goal.transform.position;
        }
    }

    // now must have person work on tree, then move resources to nearest stockpile

    /// FindClosestLocation
    ///
    /// Finds the closest location via the "FindGameObjectsWithTag" function
    /// location string must match a tag within unity for function to work
    GameObject FindClosestLocation(string location)
    {
        float distance = Mathf.Infinity;
        float curDistance = 0.0f;
        GameObject closest = null;
        GameObject[] list = GameObject.FindGameObjectsWithTag(location);
        if (list.Length <= 0)
        {
            print("No " + location);
            return null;
        }


        // NEED TO COMPARE DISTANCE "Vector3.Distance" and determine closest stockpile
        for (int i = 0; i < list.Length; i++)
        {
            curDistance = Vector3.Distance(transform.position, list[i].transform.position);
            if (curDistance < distance)
            {
                distance = curDistance;
                closest = list[i];
            }
        }
        return closest;
    }
}
