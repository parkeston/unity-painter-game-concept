using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicWandering2 : KinematicFace
{
	[SerializeField] private float wanderOffset;
	[SerializeField] private float wanderRadius;
	[SerializeField] private float wanderRate;

	private float wanderOrientation;

	protected override void Awake()
	{
		agent = GetComponent<Agent>();
		TargetAux = Target = new GameObject("wandering").transform;
	}

	protected override SteeringOutput GetSteering()
	{
		wanderOrientation += Random.Range(-1f, 1f) * wanderRate;
		Vector3 targetOrientation = Quaternion.Euler(0,wanderOrientation,0)*transform.forward;
		TargetAux.position = transform.position + wanderOffset * transform.forward + wanderRadius *targetOrientation;

		var result =  base.GetSteering();
		result.Velocity = transform.forward * agent.MaxSpeed;

		return result;
	}
}
