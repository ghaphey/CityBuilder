using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonController : MonoBehaviour
{
    [SerializeField] float workDistance = 1.0f;
    [SerializeField] int maxInventorySpace = 20;

    private enum personState { Idle, MovingItem, Building, Moving, Working };
    private enum personTask { Unassigned, Worker, Builder};

    personState currState = personState.Idle;
    personTask task = personTask.Unassigned;

    private GameObject goal = null;
    private NavMeshAgent agent;

    private int workTimeLeft = 0;
    private Inventory myInventory;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (task)
        {
            case personTask.Worker:
                workerTask();
                break;
            case personTask.Builder:
                break;
            case personTask.Unassigned:
                break;
        }
//        goal = FindClosestLocation("Gather");
 //       if (goal != null)
 //       {
 //           currState = personState.Moving;
 //           agent.destination = goal.transform.position;
  //      }
    }

    private void workerTask()
    {
        switch(currState)
        {
            case personState.Idle:
                goal = FindClosestLocation("Gather");
                if (goal != null)
                {
                    currState = personState.Moving;
                    agent.destination = goal.transform.position;
                    agent.isStopped = false;
                }
                else
                    print("Nothing to Gather!");
                break;
            case personState.Moving:
                if (goal == null)
                {
                    agent.isStopped = true;
                    currState = personState.Idle;
                }
                else if (agent.remainingDistance <= workDistance)
                {
                    currState = personState.Working;
                }
                else if (agent.isStopped == true)
                    agent.isStopped = false;
                break;
            case personState.Working:
                PersonWorking();
                break;
            case personState.MovingItem:
                if (goal == null)
                {
                    //find empty stockpile
                    //if none exist, say so

                }
                break;
            case personState.Building:
                // not implemented
                break;
        }
    }

    private void PersonWorking()
    {
        //not finished
        ResourceController resource = goal.GetComponent<ResourceController>();
        if (resource == null)
            print("can't gather not a resource");
        if (workTimeLeft < resource.GetWorkTime())
        {

        }
        else if (myInventory.num < maxInventorySpace)
        {
            goal = FindClosestLocation("Gather", myInventory.itemType);
            currState = personState.Moving;
            if (goal == null)
                currState = personState.MovingItem;
        }
        else
        {
            currState = personState.MovingItem;
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

    /// FindClosestLocation (based on resource)
    ///
    /// Finds the closest location via the "FindGameObjectsWithTag" function
    /// location string must match a tag within unity for function to work
    /// and must also match the desired resource type
    GameObject FindClosestLocation(string location, string resourceType)
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
                // MUST DO SOMETHING HERE TO MAKE IT HAPPEN
                distance = curDistance;
                closest = list[i];
            }
        }
        return closest;
    }


    private class Inventory
    {
        public int num = 0;
        public string itemType = null;
    }
}
