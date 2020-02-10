using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicLook : KinematicAlign
{
	protected override void Awake()
	{
		base.Awake();
		Target = new GameObject("Look").transform;
	}

	protected override SteeringOutput GetSteering()
	{
		Vector3 velocity = agent.Velocity;
		velocity.y = 0;

		if (velocity == Vector3.zero)
			return default;

		Target.rotation = Quaternion.Euler(0, Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, 0);
		return base.GetSteering();
	}
}
