using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//cycled or reverse moving???
public class FollowPath : KinematicSeek
{
	[SerializeField] private Transform[] nodes;
	[SerializeField] private float pathOffset;

	private Path path;
	private float currentParam;

	protected override void Awake()
	{
		base.Awake();
		path = new Path(nodes);
		Target = new GameObject("path follow").transform;
	}

	protected override SteeringOutput GetSteering()
	{
		//implement radius satisfaction?
		currentParam = path.GetParam(transform.position, currentParam);
		Target.position = path.GetPosition(currentParam + pathOffset);

		return base.GetSteering();
	}
}

//make path an innner class?
public class Path
{
	//convert ot struct (implement equals & operator overloading & get hashcode)
	private class PathSegment
	{
		public readonly Vector3 start, end;

		public PathSegment() { }
		public PathSegment(Vector3 start , Vector3 end )
		{
			this.start = start;
			this.end = end;
		}
	}

	List<PathSegment> segments;

	public Path(Transform[] nodes)
	{
		segments = new List<PathSegment>();

		for(int i = 0; i<nodes.Length-1; i++)
		{
			PathSegment segment = new PathSegment(nodes[i].position, nodes[i + 1].position);
			segments.Add(segment);
		}
	}

	public float GetParam(Vector3 position, float lastParam)
	{
		float param = 0f;
		PathSegment currentSegment = null;

		foreach(PathSegment segment in segments)
		{
			param += Vector3.Distance(segment.start, segment.end);

			if(lastParam<=param)
			{
				currentSegment = segment;
				break;
			}
		}

		if (currentSegment == null)
			return 0f;

		Vector3 pointInSegment = Vector3.Project(position-currentSegment.start, (currentSegment.end - currentSegment.start).normalized); //normalized???

		param -= Vector3.Distance(currentSegment.start, currentSegment.end);
		param += pointInSegment.magnitude;

		return param;
	}

	public Vector3 GetPosition(float currentParam)
	{
		float param = 0f;
		PathSegment currentSegment = null;

		foreach(PathSegment segment in segments)
		{
			param += Vector3.Distance(segment.start, segment.end);

			if(currentParam<=param)
			{
				currentSegment = segment;
				break;
			}
		}

		if (currentSegment == null)
			return segments[0].start;

		param -= Vector3.Distance(currentSegment.start, currentSegment.end);
		param = currentParam - param;

		return currentSegment.start + (currentSegment.end - currentSegment.start).normalized * param;
	}
}
