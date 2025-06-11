using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Game Objects")]
	[Tooltip("A reference to the BallController script.")]
	public BallController ball;
	[Tooltip("A reference to the GameObject of the second player's paddle.")]
	public GameObject paddlePlayer2;

	[Header("UI Elements")]
	[Tooltip("The TextMeshPro UI element for Player 1's score.")]
	public TextMeshProUGUI scoreTextPlayer1;
	[Tooltip("The TextMeshPro UI element for Player 2's score.")]
	public TextMeshProUGUI scoreTextPlayer2;

	[Header("Effects")]
	[Tooltip("The particle effect prefab to spawn when the goal is scored.")]
	public GameObject goalExplosionPrefab;
	[Tooltip("A reference to the ScreenShake script on the main camera.")]
	public ScreenShake mainCameraScreenShake;

	//	--- SCORES ---
	private int scorePlayer1;
	private int scorePlayer2;

	//	--- SINGLETON INSTANCE ---
	public static GameManager Instance { get; private set; }

	///	<summary>
	///	Initializes the Singleton instance.
	///	</summary>
	private void Awake()
	{
		//	Standard Singleton setup
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}

		SetupGameMode();
	}

	///	<summary>
	///	Configures the game based on the mode selected in the main menu.
	///	</summary>
	private void SetupGameMode()
	{
		//	Find the controller components on the second paddle.
		PaddleController player2Controller = paddlePlayer2.GetComponent<PaddleController>();
		PaddleAIController player2AIController = paddlePlayer2.GetComponent<PaddleAIController>();

		if (GameData.currentGameMode == GameData.GameMode.PlayerVsPlayer)
		{
			//	In PvP mode, enable the player controller and disable the AI.
			player2Controller.enabled = true;
			player2AIController.enabled = false;
		}
		else //	This means GameMode is PlayerVsCpu
		{
			//	In PvCPU mode, disable the player controller and enable the AI.
			player2Controller.enabled = false;
			player2AIController.enabled = true;

			//	The AI needs to know where the ball is, so we assign it here from code.
			player2AIController.ballTransform = ball.transform;
		}
	}

	///	<summary>
	///	Called once at the start to initialize UI.
	///	</summary>
	private void Start()
	{
		//	Initialize the UI with the starting scores.
		UpdateScoreUI();
	}

	///	<summary>
	///	Called by a GoalZone when a player scores.
	///	</summary>
	///	<param name="playerID"></param>
	public void PlayerScored(int playerID)
	{
		if(SoundManager.Instance != null)
		{
			SoundManager.Instance.PlaySound(SoundManager.Instance.goalSound);
		}

		if (playerID == 1)
		{
			scorePlayer1++;
		}
		else if (playerID == 2)
		{
			scorePlayer2++;
		}

		//	Update the visual score on the screen
		UpdateScoreUI();

		//	--- TRIGGER EFFECTS ---
		//	Istantiate the explosion at the ball's position
		if (goalExplosionPrefab != null)
		{
			Instantiate(goalExplosionPrefab, ball.transform.position, Quaternion.identity);
		}

		//	Trigger the screen shake effect
		if (mainCameraScreenShake != null)
		{
			//	We call the async method
			mainCameraScreenShake.Shake(0.15f, 0.2f).Forget();
		}

		//	Deactivate the ball to hide it. ResetBall will reactivate it.
		ball.gameObject.SetActive(false);
		ball.ResetBall();
	}

	///	<summary>
	///	Updates the score text elements on the screen
	///	</summary>
	private void UpdateScoreUI()
	{
		scoreTextPlayer1.text = scorePlayer1.ToString();
		scoreTextPlayer2.text = scorePlayer2.ToString();
	}
}