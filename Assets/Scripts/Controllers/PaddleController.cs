using UnityEngine;

public class PaddleController : MonoBehaviour
{
	[Header("Movement Settings")]
	[Tooltip("The movement speed of the paddle.")]
	public float speed = 10f;

	[Header("Boundaries")]
	[Tooltip("The maximum Y position the paddle can reach.")]
	public float maxYBoundary = 5f;
	[Tooltip("The minimum Y position the paddle can reach.")]
	public float minYBoundary = -5f;

	[Header("Input Keys")]
	[Tooltip("The key used to move the paddle upwards.")]
	public KeyCode moveUpKey = KeyCode.W;
	[Tooltip("The key used to move the paddle downwards.")]
	public KeyCode moveDownKey = KeyCode.S;

	private Rigidbody2D rb;

	///	<summary>
	///	Initializes the component by getting the Rigidbody2D reference.
	///	</summary>
	private void Start()
	{
		//	Get the Rigidbody2D component at the start
		rb = GetComponent<Rigidbody2D>();
	}

	///	<summary>
	///	Handles the physics-based movement of the paddle each fixed frame.
	///	</summary>
	private void FixedUpdate()
	{
		//	Determine movement direction from input
		Vector2 moveDirection = Vector2.zero;

		//	Check if the input keys are being pressed
		if (Input.GetKey(moveUpKey))
		{
			moveDirection = Vector2.up;
		}
		else if (Input.GetKey(moveDownKey))
		{
			moveDirection = Vector2.down;
		}

		//	Calculate the new potential position
		Vector2 newPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;

		//	Clamp the Y value of the new position within the boundaries
		newPosition.y = Mathf.Clamp(newPosition.y, minYBoundary, maxYBoundary);

		rb.MovePosition(newPosition);
	}
}