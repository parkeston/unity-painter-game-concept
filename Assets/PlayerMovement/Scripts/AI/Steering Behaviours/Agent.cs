using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SteeringOutput
{
	public Vector3 Velocity { get; set; }
	public float Rotation { get; set; }
}

public class Agent : Dummy
{
	[SerializeField] private Transform desiredTarget;

	private SteeringOutput steering;
	private SortedDictionary<int, List<(SteeringOutput singleSteering,float weight)>> groups;

	public float MaxSpeed => maxSpeed;
	public float MaxAngularSpeed => maxAngularSpeed;
	public Vector3 Velocity => controller.velocity;
	public Transform DesiredTarget => desiredTarget;
	public bool IsGrounded => controller.isGrounded;
	public float AttackTimer => attackTimer;

	private void Start()
	{
		groups = new SortedDictionary<int, List<(SteeringOutput singleSteering, float weight)>>();
	}

	private void Update()
	{
		steering = GetPrioritySteering();
		transform.rotation *= Quaternion.Euler(0, steering.Rotation * Time.deltaTime, 0);

		if (controller.isGrounded)
			velocityY = 0;
		velocityY -= gravity*Time.deltaTime * ((velocityY < 0) ? fallMultiplier : 1);

		Vector3 targetVelocity = steering.Velocity + Vector3.up * velocityY;
		controller.Move(targetVelocity* Time.deltaTime);
		DummyAnimator.SetFloat("Speed", controller.velocity.magnitude);
	}

	private void LateUpdate() //virtual?
	{
		//if agent behavour is disabled, reset Steering to stop moving
		groups.Clear();
		steering = default;
	}

	public void SetSteering(SteeringOutput result,int priority, float weight)
	{
		if (!groups.ContainsKey(priority))
			groups.Add(priority, new List<(SteeringOutput,float)>());
		groups[priority].Add((result,weight));
	}

	private SteeringOutput GetPrioritySteering()
	{
		SteeringOutput result = default;

		foreach(var group in groups.Values)
		{
			result = default;

			foreach(var tuple in group)
			{
				result.Velocity = Vector3.ClampMagnitude(result.Velocity + tuple.singleSteering.Velocity*tuple.weight, maxSpeed);
				result.Rotation = Mathf.Clamp(result.Rotation + tuple.singleSteering.Rotation*tuple.weight, -maxAngularSpeed, maxAngularSpeed);
			}
			if (result.Velocity.sqrMagnitude > Mathf.Epsilon || Mathf.Abs(result.Rotation) > Mathf.Epsilon)
				return result;
		}

		return result;
	}
}
