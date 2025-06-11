using UnityEngine;

public class GoalZoneController : MonoBehaviour
{
	[Header("Configuration")]
	[Tooltip("Set this to the ID of the player who scores when the ball enters this zone. (e.g. 1 or 2)")]
	public int scoringPlayerID;

	///	<summary>
	///	Called by Unity's physics engine when another collider enter this trigger.
	///	</summary>
	///	<param name="collision">The collider that entered the trigger,</param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//	First, check if the object that entered is the ball.
		if (collision.CompareTag("Ball"))
		{
			//	If it is the ball, notify the GameManager that a point was scored.
			//	We access the GameManager through its static Instance property.
			GameManager.Instance.PlayerScored(scoringPlayerID);
		}
	}
}