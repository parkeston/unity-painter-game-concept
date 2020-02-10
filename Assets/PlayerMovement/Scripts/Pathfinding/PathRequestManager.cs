using System;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{

	private Queue<PathRequest> pathRequestQueue;
	private PathRequest currentRequest;

	private static PathRequestManager instance;
	private Pathfinding pathfinding;

	private bool isProcessing;

	private void Awake()
	{
		instance = this;
		pathfinding = GetComponent<Pathfinding>();
		pathRequestQueue = new Queue<PathRequest>();
	}

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[],bool> callback)
	{
		PathRequest newReqest = new PathRequest(pathStart, pathEnd, callback);
		instance.pathRequestQueue.Enqueue(newReqest);
		instance.TryProcessNext();
	}

	private void TryProcessNext()
	{
		if(!isProcessing &&pathRequestQueue.Count>0)
		{
			currentRequest = pathRequestQueue.Dequeue();
			isProcessing = true;
			pathfinding.StartFindPath(currentRequest.PathStart, currentRequest.PathEnd);
		}
	}

	public void FinishedProcessing(Vector3[] path, bool success)
	{
		currentRequest.Callback.Invoke(path, success);
		isProcessing = false;
		TryProcessNext();
	}

	private struct PathRequest
	{
		public Vector3 PathStart { get; set; }
		public Vector3 PathEnd { get; set; }
		public Action<Vector3[], bool> Callback;

		public PathRequest(Vector3 start, Vector3 end, Action<Vector3[],bool> callback)
		{
			PathStart = start;
			PathEnd = end;
			Callback = callback;
		}
	}

}
