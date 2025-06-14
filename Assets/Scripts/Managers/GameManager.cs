using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("Game Rules")]
	[Tooltip("The score a player needs to reach to win the match.")]
	public int scoreToWin = 5;

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
	[Tooltip("The panel that appears when the game is over.")]
	public GameObject gameOverPanel;
	[Tooltip("The text element that displays the winner.")]
	public TextMeshProUGUI winnerText;

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
	///	Called once at the start to initialize UI.
	///	</summary>
	private void Start()
	{
		//	Initialize the UI with the starting scores.
		UpdateScoreUI();
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
	///	Called by a GoalZone when a player scores.
	///	</summary>
	///	<param name="playerID"></param>
	public void PlayerScored(int playerID)
	{
		if(playerID != 0)
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

			UpdateScoreUI();

			//	Check for a winner
			if (scorePlayer1 >= scoreToWin)
			{
				EndGame(1);
			}
			else if (scorePlayer2 >= scoreToWin)
			{
				EndGame(2);
			}
			else
			{
				//	If no one has won yet, reset the round
				TriggerRoundEffects();
				ball.ResetBall();
			}
		}
	}

	///	<summary>
	///	Updates the score text elements on the screen
	///	</summary>
	private void UpdateScoreUI()
	{
		scoreTextPlayer1.text = scorePlayer1.ToString();
		scoreTextPlayer2.text = scorePlayer2.ToString();
	}

	///	<summary>
	///	Triggers all visual and audio effects for a scored point.
	///	</summary>
	private void TriggerRoundEffects()
	{
		if (goalExplosionPrefab != null)
		{
			Instantiate(goalExplosionPrefab, ball.transform.position, Quaternion.identity);
		}

		if (mainCameraScreenShake != null)
		{
			mainCameraScreenShake.Shake(0.15f, 0.2f).Forget();
		}

		ball.gameObject.SetActive(false);
	}

	///	<summary>
	///	Handles the end of the game state by showing the game over panel and freezing time.
	///	</summary>
	///	<param name="winnerID">The ID of the player who won.</param>
	private void EndGame(int winnerID)
	{
		winnerText.text = $"PLAYER {winnerID} WINS!";
		gameOverPanel.SetActive(true);

		//	Freeze the game by stopping time.
		Time.timeScale = 0f;
	}

	///	<summary>
	///	Called by the 'Restart' button on the GameOver panel.
	///	</summary>
	public void OnRestartButtonClicked()
	{
		//	Play UI click sound
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.PlaySound(SoundManager.Instance.uiClickSound);
		}

		//	Unfreeze the game before reloading the scene.
		Time.timeScale = 1f;

		//	Reload the current scene.
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	///	<summary>
	///	Called by the 'Main Menu' button on the GameOver panel.
	///	</summary>
	public void OnMainMenuButtonClicked()
	{
		//	Play UI click sound
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.PlaySound(SoundManager.Instance.uiClickSound);
		}

		//	Unfreeze the game before loading the main menu.
		Time.timeScale = 1f;
	}
}