using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidAgent : AgentBehaviour
{
	[SerializeField] private float collisionRadius = 0.5f;
	private GameObject[] targets;

	private void Start()
	{
		targets = GameObject.FindGameObjectsWithTag("Agent");
	}

	//clamp by distance of finding targets?
	//clamp by time to collision?
	protected override SteeringOutput GetSteering()
	{
		SteeringOutput result = default;

		float shortestTime = Mathf.Infinity;
		GameObject firstTarget = null;
		float firstMinSeparation = 0f;
		float firstDistance = 0f;
		Vector3 firstRelativePos = Vector3.zero;
		Vector3 firstRelativeVelocity = Vector3.zero;

		foreach(GameObject possibleTarget in targets)
		{
			Agent targetAgent = possibleTarget.GetComponent<Agent>();
			Vector3 relativePos = possibleTarget.transform.position-transform.position;
			Vector3 relativeVelocity = targetAgent.Velocity-agent.Velocity;
			float relativeSpeed = relativeVelocity.magnitude;
			float timeToCollision = Vector3.Dot(relativePos, relativeVelocity) / (relativeSpeed * relativeSpeed)*-1f; //-1?

			float distance = firstRelativePos.magnitude;
			float minSeparation = distance - relativeSpeed * timeToCollision;
			if (minSeparation > 2 * collisionRadius)
				continue;

			if(timeToCollision>0f && timeToCollision<shortestTime)
			{
				shortestTime = timeToCollision;
				firstTarget = possibleTarget;
				firstMinSeparation = minSeparation;
				firstDistance = distance;
				firstRelativePos = relativePos;
				firstRelativeVelocity = relativeVelocity;
			}
		}

		if (firstTarget == null)
			return result;

		if (firstMinSeparation <= 0f || firstDistance < 2 * collisionRadius)
			firstRelativePos = firstTarget.transform.position - transform.position;
		else
			firstRelativePos += firstRelativeVelocity * shortestTime;

		firstRelativePos.Normalize();

		result.Velocity = -firstRelativePos * agent.MaxSpeed;

		return result;
	}
}
