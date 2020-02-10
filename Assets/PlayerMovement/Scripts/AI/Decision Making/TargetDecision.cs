using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsTargetInRange", menuName = "Decision Making/Decision/IsTargetInRange", order = 1)]
public class TargetDecision : Decision<float>
{
	[SerializeField] private float range;

	public override DecisionTreeNode GetBranch()
	{
		//constraint attack range to arrive target radius?
		if (TestValue() <= range)
			return trueNode;
		return falseNode;
	}

	public override float TestValue()
	{
		if (agent.DesiredTarget == null)
			return 1000;

		Vector3 distance = agent.DesiredTarget.position - agent.transform.position;
		distance.y = 0;
		return distance.magnitude;
	}
}
