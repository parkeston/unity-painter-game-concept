using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicArrive : AgentBehaviour
{
	[SerializeField] private float targetRadius = 1f;
	[SerializeField] private float timeToTarget = 1f;


	//smooth movement, account existing timetoTarget logic
	//slerp vs smoothdamp - how vectors are treated
	//find alternative to timeToTarget speed slowing

	protected override SteeringOutput GetSteering()
	{
		SteeringOutput steering = default;

		if (Target == null)
			return default;

		Vector3 direction = Target.position - transform.position;
		direction.y = 0;

		if (direction.magnitude <= targetRadius)
			return default;

		Vector3 desiredVelocity = direction / timeToTarget;
		desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, agent.MaxSpeed);

		steering.Velocity = desiredVelocity;

		return steering;
	}
}
