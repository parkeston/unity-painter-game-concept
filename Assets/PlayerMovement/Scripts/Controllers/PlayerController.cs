using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Dummy
{
	[SerializeField] private float timeToTurn = 0.1f;
	[SerializeField] private float jumpHeight = 2;
	[SerializeField] private float deadAngle = 150f;
	[Range(0, 1)] [SerializeField] private float airControlFactor = 0.3f;
	[Range(0, 1)] [SerializeField] private float slowDownFactor = 0.7f;
	[SerializeField] private float heightPadding = 0.2f;

	private float jumpSpeed;
	private float timer;

	private Transform camTransform;
	private Vector2 inputDir;
	private Vector2 beforeFall = Vector2.zero;

	private bool isJumping;
	private bool isGrounded;

	Quaternion targetRotation;

	protected override void Awake()
	{
		base.Awake();
		jumpSpeed = Mathf.Sqrt(2 * gravity * jumpHeight);
		camTransform = Camera.main.transform;
	}

	//---- Problems to solve ----
	//check prev cam & input state for  rotation optimizing!
	//input manager settings, eliminating slippery feel & etc. (GetAxisRaw do not affected by input manager settings!)
	//split input logic from controller logic?

	private void Update()
	{
		if (DummyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
			return;

		inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * maxSpeed;
		inputDir = Vector2.ClampMagnitude(inputDir, maxSpeed); //clamping on air & ground, not affecting jump height (inputDir is separate from jump value)

		isGrounded = CheckGround();
		if (isGrounded)
		{
			isJumping = false;
			velocityY = 0;

			if (Input.GetButtonDown("Jump"))
			{
				velocityY = jumpSpeed;
				isJumping = true;
			}
			
			beforeFall = inputDir;
		}
		else
		{
			float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camTransform.rotation.eulerAngles.y;
			targetRotation = Quaternion.Euler(0, rotation, 0);
			Vector3 target = targetRotation * Vector3.forward;

			if (Vector3.Angle(transform.forward.normalized, target.normalized)>=deadAngle)
				inputDir = beforeFall*slowDownFactor;
		}

		CalculateRotation();

		velocityY -= gravity * Time.deltaTime * ((velocityY < 0) ? fallMultiplier : 1); //gravity applied per second
		Vector3 targetMove = targetRotation*Vector3.forward* inputDir.magnitude + Vector3.up * velocityY;
		controller.Move(targetMove * Time.deltaTime); //movement per second

		PlayAnimation();
	}

	private float GetRotation(float current, float target)
	{
		float angularSpeed = maxAngularSpeed;
		if (!isGrounded)
			angularSpeed *= airControlFactor;

		float delta = Mathf.DeltaAngle(current, target);
		delta /= timeToTurn;
		delta = Mathf.Clamp(delta, -angularSpeed, angularSpeed);
		return delta;
	}

	private bool CheckGround()
	{
		if (controller.isGrounded)
			return true;

		if (isJumping)
			return false;

		//slope check
		if (Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit, heightPadding))
		{
			//delegate to final movement vector
			controller.Move(Vector3.down * hit.distance);
			return true;
		}

		return false;
	}

	private void CalculateRotation()
	{
		if (inputDir != Vector2.zero)
		{
			float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camTransform.rotation.eulerAngles.y;
			targetRotation = Quaternion.Euler(0, rotation, 0);

			rotation = GetRotation(transform.rotation.eulerAngles.y, rotation);
			transform.rotation *= Quaternion.Euler(0, rotation * Time.deltaTime, 0);

			if (!isGrounded)
				targetRotation = transform.rotation;
		}
	}

	private void PlayAnimation()
	{
		DummyAnimator.SetBool("IsGrounded", isGrounded);
		DummyAnimator.SetFloat("HSpeed", inputDir.magnitude);
		DummyAnimator.SetFloat("VSpeed", velocityY);

		if (Input.GetButtonDown("Fire1") && isGrounded && Time.time >= timer)
		{
			if (CurrentPaint.Amount <= 0)
				return;
			DummyAnimator.SetTrigger("Attack");
			timer = Time.time + attackTimer;
		}
	}
}
