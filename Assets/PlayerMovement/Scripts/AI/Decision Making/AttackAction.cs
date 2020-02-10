using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Decision Making/Action/Attack", order = 1)]
public class AttackAction : AgentAction
{
	private float timer;

	//to not save timer value after session
	private void OnEnable()
	{
		timer = 0f;
	}

	public override void Execute()
	{
		//play attack anim
		if (Time.time >= timer)
		{
			agent.DummyAnimator.SetTrigger("Attack");
			timer = Time.time + agent.AttackTimer;
		}

		//Debug.Log($"Attacking {agent.DesiredTarget.name}");
	}
}
