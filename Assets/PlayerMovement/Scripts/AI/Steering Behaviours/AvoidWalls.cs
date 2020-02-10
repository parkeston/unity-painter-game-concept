using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidWalls : KinematicSeek
{
	[SerializeField] private float avoidDistance;
	[SerializeField] private float lookahed;
	[SerializeField] private float offset;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private float targetRadius = 0.1f;
	[SerializeField] private float whiskersAngle = 35f;

	private bool seekToTarget;

	private Ray frontCenter;
	private Ray leftWhisker;
	private Ray rightWhisker;
	private RaycastHit hit;

	private float avoidMultiplier;
	private bool avoiding;

	protected override void Awake()
	{
		base.Awake();
		Target = new GameObject("avoid walls").transform;
	}

	protected override SteeringOutput GetSteering()
	{
		if (seekToTarget)
		{
			var direction = Target.position - transform.position;
			direction.y = 0;

			if(direction.magnitude>targetRadius)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction.normalized), agent.MaxAngularSpeed * Time.deltaTime);
				return base.GetSteering();
			}
			else
				seekToTarget = false;
		}

		Vector3 velocity = agent.Velocity;
		velocity.y = 0;
		if (velocity == Vector3.zero || !agent.IsGrounded)
			return default;

		frontCenter = new Ray(transform.position + Vector3.up * offset, velocity.normalized);
		leftWhisker = new Ray(transform.position + Vector3.up * offset, Quaternion.Euler(0, -whiskersAngle, 0) * velocity.normalized);
		rightWhisker = new Ray(transform.position + Vector3.up * offset, Quaternion.Euler(0, whiskersAngle, 0) * velocity.normalized);
		Debug.DrawRay(leftWhisker.origin, leftWhisker.direction * lookahed, Color.red);
		Debug.DrawRay(rightWhisker.origin, rightWhisker.direction * lookahed, Color.red);
		Debug.DrawRay(frontCenter.origin, frontCenter.direction * lookahed, Color.red);

		avoiding = false;
		avoidMultiplier = 0f;

		if (Physics.Raycast(leftWhisker, out hit, lookahed, layerMask))
		{
			avoiding = true;
			avoidMultiplier += 1f;
		}

		if (Physics.Raycast(rightWhisker, out hit, lookahed, layerMask))
		{
			avoiding = true;
			avoidMultiplier -= 1f;
		}

		if (Physics.Raycast(frontCenter, out hit, lookahed, layerMask))
		{
			if (avoiding && avoidMultiplier == 0f)
			{
				print("wall facin!");
				avoidMultiplier = 2f;
			}
			else if (!avoiding && hit.distance <= Mathf.Cos(whiskersAngle*Mathf.Deg2Rad)*lookahed)
			{
				print("thin obst");
				avoidMultiplier = 2f;
				avoiding = true;
			}
		}
	

		if (avoiding)
		{
			Target.position = transform.position + Quaternion.Euler(0, whiskersAngle * avoidMultiplier, 0) * velocity.normalized*avoidDistance;

			var direction = Target.position - transform.position;
			direction.y = 0;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction.normalized), agent.MaxAngularSpeed * Time.deltaTime);
			seekToTarget = true;
			return base.GetSteering();
		}
		else
			return default;
		
	}
}
