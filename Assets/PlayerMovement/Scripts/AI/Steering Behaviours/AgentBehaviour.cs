using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
abstract public class AgentBehaviour : MonoBehaviour
{
	[SerializeField] private float weight;
	[SerializeField] private int priority;

	public Transform Target { get; set; }
	protected Agent agent;

	protected virtual void Awake()
	{
		agent = GetComponent<Agent>();
	}

	private void Update()
	{
		agent.SetSteering(GetSteering(), priority,weight);
	}

	abstract protected SteeringOutput GetSteering();
}