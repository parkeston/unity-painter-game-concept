using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Paint
{
	public readonly Color Color;
	public int Amount { get; set; }

	//delegate to default weapon ammo size
	public  Paint(Color color, int amount = 20)
	{
		Color = color;
		Amount = amount;
	}
}

public class ColorSwitcher : MonoBehaviour
{
	[SerializeField] private Color[] colors;

	//hotfix references
	[SerializeField] private Image colorIndicator;
	[SerializeField] private TextMeshProUGUI ammoIndicator;
	[SerializeField] private TrailRenderer trailRenderer;

	public TextMeshProUGUI AmmoIndicator => ammoIndicator;

	private Dummy dummy;
	private int targetIndex;

	public Paint[] Paints { get; private set; }

	private void Awake()
	{
		dummy = GetComponent<Dummy>();
		Paints = new Paint[colors.Length];

		for(int i = 0;i<Paints.Length;i++)
		{
			Paints[i] = new Paint(colors[i]);
		}
	}

	private void Start()
	{
		dummy.CurrentPaint = Paints[0];
		UpdateAll();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			targetIndex++;
			if (targetIndex >= colors.Length)
				targetIndex = 0;

			dummy.CurrentPaint = Paints[targetIndex];

			UpdateAll();
		}
	}

	public void UpdateAmmo()
	{
		ammoIndicator.text = dummy.CurrentPaint.Amount.ToString();
	}

	public void UpdateAll()
	{
		trailRenderer.material.color = dummy.CurrentPaint.Color;
		UpdateAmmo();
		colorIndicator.color = dummy.CurrentPaint.Color;
	}
}
