using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "Decision Making/Action/Idle", order = 1)]
public class IdleAction : AgentAction
{
	public override void Execute()
	{
		agent.GetComponent<KinematicArrive>().Target = null;
		//gets first component with specified base class, so get can not what you want
		agent.GetComponent<KinematicFace>().TargetAux = null;
	}
}
