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
    public void ReceivePathfinding(Pathfinding pathfindingCandidate)
    {
        if(pathfindingCandidate != null)
        {
            pathfinding = pathfindingCandidate;
        }
    }
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
    public void GiveResult(Vector3[] path, bool success)
    {
        requests.Dequeue().callbackFunction(path, success);
        processingRequest = false;
        ProcessRequest();
    }
    public void EndPathfinding()
    {
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
