using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathfindingManager : MonoBehaviour
{
    Queue<Request> requests = new Queue<Request>();
    bool processingRequest;
    Pathfinding pathfinding;
    void Start()
    {
        if(GameManager.manager.pathManager == null && GameManager.manager.pathManager != this)
        {
            GameManager.manager.pathManager = this;
            DontDestroyOnLoad(gameObject);
            GameManager.manager.pathfindingLoaded = true;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
    //pathfinding registers itself with the manager here
    public void ReceivePathfinding(Pathfinding pathfindingCandidate)
    {
        if(pathfindingCandidate != null)
        {
            pathfinding = pathfindingCandidate;
        }
    }
    //entities can call this function to queue up a request a path between two points, callback function params (Vector3[], bool)
    public void RequestPath(Vector3 start, Vector3 goal, Action<Vector3[], bool> callbackFunction)
    {
        requests.Enqueue(new Request(start, goal, callbackFunction));
        ProcessRequest();
    }
    void ProcessRequest()
    {
        if(!processingRequest)
        {
            if(requests.Count > 0)
            {
                processingRequest = true;
                Request request = requests.Peek();
                pathfinding.ProcessRequest(request.start, request.goal);
            }
        }
    }
    //send the result back to the waiting entity
    public void GiveResult(Vector3[] path, bool success)
    {
        requests.Dequeue().callbackFunction(path, success);
        processingRequest = false;
        ProcessRequest();
    }
    public void EndPathfinding()
    {
        processingRequest = false;
        pathfinding.EndPathfinding();
        requests = new Queue<Request>();
    }
    struct Request
    {
        public Vector3 start;
        public Vector3 goal;
        public Action<Vector3[], bool> callbackFunction;
        public Request(Vector3 start, Vector3 goal, Action<Vector3[], bool> callbackFunction)
        {
            this.start = start;
            this.goal = goal;
            this.callbackFunction = callbackFunction;
        }
    }
}
