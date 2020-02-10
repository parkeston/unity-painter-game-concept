using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class WeaponScript : MonoBehaviour
{
	public Dummy Owner { get; private set; }

	[SerializeField] private float damage;
	[SerializeField] private int attackCost = 5;
	public float Damage => damage;
	public int Attackcost => attackCost;

	private void Awake()
	{
		Owner = GetComponentInParent<Dummy>();
	}
}
