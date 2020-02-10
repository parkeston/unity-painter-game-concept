using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase", menuName = "Decision Making/Action/Chase", order = 1)]
public class ChaseAction : AgentAction
{
	public override void Execute()
	{
		//play anim, or embedded in locomotion?
		agent.GetComponent<KinematicArrive>().Target = agent.DesiredTarget;
		agent.GetComponent<KinematicFace>().TargetAux = agent.DesiredTarget;
		//Debug.Log($"Chasing {agent.DesiredTarget.name}");
	}
}
