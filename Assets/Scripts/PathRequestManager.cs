using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> prQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    static PathRequestManager instance;
    Pathfinding pathFinding;

    bool isProcessingPath;

    public void Awake()
    {
        instance = this;
        pathFinding = GetComponent<Pathfinding>();
    }

    struct PathRequest
    {
        public Vector3 start;
        public Vector3 end;
        public Action<Vector3[], bool> callback;

        public PathRequest ( Vector3 _start, Vector3 _end, Action<Vector3[],bool> _callback)
        {
            start = _start;
            end = _end;
            callback = _callback;
        }
    }
    public static void RequesPath(Vector3 start, Vector3 end, Action<Vector3[],bool> callback)
    {
        PathRequest newRequest = new PathRequest(start, end, callback);
        instance.prQueue.Enqueue(newRequest);
        instance.tryProcessNext();
        
    }

    private void tryProcessNext()
    {
        if (!isProcessingPath && prQueue.Count > 0)
        {
            currentRequest = prQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentRequest.start, currentRequest.end);
        }
    }

    public void FinishProcessingPath(Vector3[] path , bool success)
    {
        currentRequest.callback(path, success);
        isProcessingPath = false;
        tryProcessNext();
    }
}
