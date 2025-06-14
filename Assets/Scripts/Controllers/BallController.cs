using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class BallController : MonoBehaviour
{
	[Header("Ball Properties")]
	[Tooltip("The initial speed of the ball when launched.")]
	public float startSpeed = 7f;
	[Tooltip("The maximum speed the ball can reach.")]
	public float maxSpeed = 25f;
	[Tooltip("The minimum vertical velocity to prevent horizontal locks")]
	public float minVerticalVelocity = 0.5f;
	[Tooltip("The minimum horizontal velocity to prevent vertical locks.")]
	public float minHorizontalVelocity = 0.5f;

	[Header("Effects")]
	[Tooltip("Speed required for the trail to become visible")]
	public float trailSpeedThreshold = 12f;

	[Header("Safety Nets")]
	[Tooltip("If the ball's position exceeds this value on any axis, it will be reset.")]
	public float outOfBoundsThreshold = 15f;

	private Rigidbody2D rb;
	private TrailRenderer trail;
	private bool isResetting = false;

	///	<summary>
	///	Initializes the component and launches the ball after a short delay.
	///	</summary>
	private async void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		trail = GetComponent<TrailRenderer>();

		//	Ensure the trail is disable at the start
		if(trail != null)
		{
			trail.emitting = false;
		}

		//	We wait for 1 second before the game starts.
		await UniTask.Delay(TimeSpan.FromSeconds(1f));

		LaunchBall();
	}

	///	<summary>
	///	Checks the ball's speed every frame to control the trail emitter.
	///	</summary>
	private void Update()
	{
		//	If the ball is way outside the play area and not already resetting, trigger a reset.
		if (!isResetting && (Mathf.Abs(transform.position.x) > outOfBoundsThreshold || Mathf.Abs(transform.position.y) > outOfBoundsThreshold))
		{
			//	Call the GameManager to handle the reset without awarding points.
			if (GameManager.Instance != null)
			{
				GameManager.Instance.PlayerScored(0);
			}
			else
			{
				//	Fallback in case the GameManager is not present.
				ResetBall();
			}
		}

		if (trail == null)
		{
			return;
		}

		//	Check the speed and enable/disable the trail
		if(rb.linearVelocity.magnitude > trailSpeedThreshold)
		{
			trail.emitting = true;
		}
		else
		{
			trail.emitting = false;
		}
	}

	///	<summary>
	///	Launches the ball in a random direction with its starting speed.
	///	</summary>
	private void LaunchBall()
	{
		// Choose a random horizontal and vertical direction.
		float xDirection = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
		float yDirection = UnityEngine.Random.Range(-0.8f, 0.8f);

		Vector2 launchDirection = new Vector2(xDirection, yDirection).normalized;
		rb.linearVelocity = launchDirection * startSpeed;
	}

	///	<summary>
	///	Resets the ball to the center, waits for a delay, and then relaunches it.
	///	</summary>
	public async void ResetBall()
	{
		//	Flag that a reset is in progress to prevent multiple calls.
		isResetting = true;

		//	Stop and center the ball.
		rb.linearVelocity = Vector2.zero;
		transform.position = Vector2.zero;

		await UniTask.Delay(TimeSpan.FromSeconds(1f));

		//	Reactivate the GameObject before launching again.
		gameObject.SetActive(true);
		
		//	After the delay, call the common launch method.
		LaunchBall();

		//	Flag that the reset process is complete.
		isResetting = false;
	}

	///	<summary>
	///	Called by the physics engine when the ball collides with another collider.
	///	</summary>
	///	<param name="collision">The collision data associated with this event.</param>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		//	If it hits an object with the "Paddle" tag, slightly increase its speed
		if (collision.gameObject.CompareTag("Paddle"))
		{
			if(SoundManager.Instance != null)
			{
				SoundManager.Instance.PlaySound(SoundManager.Instance.paddleHitSound);
			}

			rb.linearVelocity *= 1.05f;

			//	Max Speed Check
			if (rb.linearVelocity.magnitude > maxSpeed)
			{
				rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
			}
		}
		else if (collision.gameObject.CompareTag("Wall"))
		{
			if(SoundManager.Instance != null)
			{
				SoundManager.Instance.PlaySound(SoundManager.Instance.wallHitSound);
			}
		}

		//	Safety checks to prevent locks
		Vector2 currentVelocity = rb.linearVelocity;
		float randomNudge = UnityEngine.Random.Range(0, 2) == 0 ? 1f : -1f;

		//	Check if the ball is moving too horizontally (stuck in the middle)
		if (Mathf.Abs(currentVelocity.y) < minVerticalVelocity)
		{
			currentVelocity.y = randomNudge * minVerticalVelocity;
		}

		//	Check if the ball is moving too vertically (stuck on the sides)
		if (Mathf.Abs(currentVelocity.x) < minHorizontalVelocity)
		{
			currentVelocity.x = randomNudge * minHorizontalVelocity;
		}

		//	Apply the corrected velocity
		rb.linearVelocity = currentVelocity.normalized * rb.linearVelocity.magnitude;
	}
}