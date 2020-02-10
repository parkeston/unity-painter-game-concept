using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class HitDetectorScript : MonoBehaviour
{
	private Dummy dummy;
	[SerializeField] private string target;
	[SerializeField] private Slider healthBar;
	[SerializeField] private ParticleSystem damageParticle;

	private void Awake()
	{
		dummy = GetComponentInParent<Dummy>();
	}

	private void Start()
	{
		if (healthBar != null)
			healthBar.value = dummy.Health;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == target)
		{
			var weapon = other.GetComponent<WeaponScript>();
			float damage = weapon.Damage;

			if(target == "PlayerWeapon")
			{
				if (dummy.CurrentPaint.Color == weapon.Owner.CurrentPaint.Color)
					damage /= 2f;

				//struct restrictions
				weapon.Owner.CurrentPaint.Amount -= weapon.Attackcost;
				weapon.Owner.GetComponent<ColorSwitcher>().UpdateAmmo();
			}

			dummy.Health -= damage;

			if(healthBar!=null)
				healthBar.value = dummy.Health;

			damageParticle?.Play();

			Debug.Log($"{dummy.name} was hitted with {damage} damage!");

			if (dummy.Health <= 0)
			{
				if(target == "PlayerWeapon")
				{
					
					var enemyPaint = dummy.CurrentPaint;
					var paints = weapon.Owner.GetComponent<ColorSwitcher>().Paints;

					foreach(var paint in paints)
						if (enemyPaint.Color == paint.Color)
							paint.Amount += enemyPaint.Amount;

					weapon.Owner.GetComponent<ColorSwitcher>().UpdateAmmo();
				}
				Destroy(dummy.gameObject);
			}
		}
	}
}
