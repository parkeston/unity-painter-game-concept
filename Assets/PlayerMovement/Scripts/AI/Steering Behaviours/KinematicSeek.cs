using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSeek : AgentBehaviour
{
	//implement character orientation ?
	protected override SteeringOutput GetSteering()
	{
		SteeringOutput result = default;

		Vector3 veloctiy = Target.position - transform.position;
		veloctiy.y = 0;

		result.Velocity = veloctiy.normalized;
		result.Velocity *= agent.MaxSpeed;

		return result;
	}
}
