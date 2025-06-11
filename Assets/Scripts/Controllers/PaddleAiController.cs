using UnityEngine;

public class PaddleAIController : MonoBehaviour
{
	[Header("AI Settings")]
	[Tooltip("The maximum speed at which the AI paddle can move.")]
	public float maxSpeed = 15f;
	[Tooltip("Time in seconds for the paddle to smoothly catch up to the ball. Lower is faster/sharper.")]
	public float smoothTime = 0.1f;
	[Tooltip("A reference to the ball's Transform to follow.")]
	public Transform ballTransform;

	[Header("Boundaries")]
	[Tooltip("The maximum Y position the paddle can reach.")]
	public float maxYBoundary = 3.7f;
	[Tooltip("The minimum Y position the paddle can reach.")]
	public float minYBoundary = -3.7f;

	private Rigidbody2D rb;
	private float currentYVelocity;

	///	<summary>
	///	Initializes the component by getting the Rigidbody2D reference.
	///	</summary>
	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	///	<summary>
	///	Handles the physics-based movement of the AI paddle each fixed frame.
	///	</summary>
	private void FixedUpdate()
	{
		//	If the ball reference is not set, do nothing.
		if (ballTransform == null)
		{
			return;
		}

		//	Calculate the new smoothed Y position using SmoothDamp.
		float targetY = ballTransform.position.y;
		float newY = Mathf.SmoothDamp(
			rb.position.y,
			targetY,
			ref currentYVelocity,
			smoothTime,
			maxSpeed
		);

		//	Clamp the new Y position to stay within the game boundaries.
		float clampedY = Mathf.Clamp(newY, minYBoundary, maxYBoundary);

		//	Create the final position vector.
		Vector2 newPosition = new Vector2(rb.position.x, clampedY);

		//	Move the Rigidbody to the new, smoothed, and clamped position.
		rb.MovePosition(newPosition);
	}
}