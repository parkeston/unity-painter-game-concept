using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
	private AgentAction oldAction;
	private AgentAction newAction;
	private Agent agent; //how to get acess to agent from decisions & actions, that are connected to root, but not on the gameobject

	[SerializeField] private DecisionTreeNode root;

	private void Awake()
	{
		agent = GetComponent<Agent>();
	}

	private void Update()
	{
		oldAction = newAction;
		newAction = root.MakeDecision(agent) as AgentAction;
		if (newAction == null)
			newAction = oldAction;

		newAction.Execute();
	}

}

//cannot use interface cause of scriptable object base class serialization needed
public abstract class DecisionTreeNode : ScriptableObject
{
	protected Agent agent;
	public abstract DecisionTreeNode MakeDecision(Agent agent);
}


public abstract class AgentAction : DecisionTreeNode
{
	public abstract void Execute();

	public override sealed DecisionTreeNode MakeDecision(Agent agent)
	{
		this.agent = agent;
		return this;
	}
}

public abstract class Decision<T> : DecisionTreeNode
{
	[SerializeField] protected DecisionTreeNode trueNode;
	[SerializeField] protected DecisionTreeNode falseNode;

	public abstract T TestValue();
	public abstract DecisionTreeNode GetBranch();

	public override sealed DecisionTreeNode MakeDecision(Agent agent)
	{
		this.agent = agent;
		return GetBranch().MakeDecision(agent);
	}
}