using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicAlign : AgentBehaviour
{
	[SerializeField] private float targetRadius;
	[SerializeField] private float timeToTarget = 0.5f;

	protected override SteeringOutput GetSteering()
	{
		SteeringOutput steering = default;

		//to map result to (-pi,pi) anlge
		float rotation = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, Target.rotation.eulerAngles.y);
		if (Mathf.Abs(rotation) < targetRadius)
			return steering;

		float angularSpeed = rotation / timeToTarget;
		angularSpeed = Mathf.Clamp(angularSpeed, -agent.MaxAngularSpeed, agent.MaxAngularSpeed);

		steering.Rotation = angularSpeed;

		return steering;
	}
}
