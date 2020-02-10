using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicFace : KinematicAlign
{
	public Transform TargetAux { get; set; }

	protected override void Awake()
	{
		base.Awake();
		TargetAux = Target;
		Target = new GameObject("face").transform;
	}

	protected override SteeringOutput GetSteering()
	{
		if (TargetAux == null)
			return default;

		Vector3 direction = TargetAux.position - transform.position;

		Target.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg, 0);

		return base.GetSteering();
	}

	private void OnDestroy()
	{
		if(Target!=null)
			Destroy(Target.gameObject);
	}
}
