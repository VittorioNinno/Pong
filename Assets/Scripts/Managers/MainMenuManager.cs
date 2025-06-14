using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	[Header("Menu Panels")]
	[Tooltip("The main panel with Play, Settings, Exit buttons.")]
	public GameObject mainMenuPanel;
	[Tooltip("The sub-panel for selecting game mode (PvP, PvCPU).")]
	public GameObject playSubmenuPanel;
	[Tooltip("The panel for game settings.")]
	public GameObject settingsPanel;

	///	<summary>
	///	Called from the 'Play' button. Opens the game mode selection sub-menu.
	///	</summary>
	public void OnPlayButtonClicked()
	{
		mainMenuPanel.SetActive(false);
		playSubmenuPanel.SetActive(true);
	}

	///	<summary>
	///	Called from the 'Settings' button. Opens the settings panel.
	///	</summary>
	public void OnSettingsButtonClicked()
	{
		mainMenuPanel.SetActive(false);
		settingsPanel.SetActive(true);
	}

	///	<summary>
	///	Called from the 'Exit' button. Closes the application.
	///	</summary>
	public void OnExitButtonClicked()
	{
#if UNITY_EDITOR
		//	If we are running in the Unity Editor
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		//	If we are in a built game
		Application.Quit();
	}

	///	<summary>
	///	Called from any 'Back' button. Returns to the main menu panel.
	///	</summary>
	public void OnBackButtonClicked()
	{
		playSubmenuPanel.SetActive(false);
		settingsPanel.SetActive(false);
		mainMenuPanel.SetActive(true);
	}

	/// <summary>
	/// Called from the 'Player VS Player' button. Sets the mode and loads the game scene.
	/// </summary>
	public void OnPlayerVsPlayerClicked()
	{
		// Set the game mode in our static data holder
		GameData.currentGameMode = GameData.GameMode.PlayerVsPlayer;
	}

	///	<summary>
	///	Called from the 'Player VS CPU' button. Sets the mode and loads the game scene.
	///	</summary>
	public void OnPlayerVsCpuClicked()
	{
		//	Set the game mode in our static data holder
		GameData.currentGameMode = GameData.GameMode.PlayerVsCpu;
	}

	public void OnButtonClicked()
	{
		if(SoundManager.Instance != null)
		{
			SoundManager.Instance.PlaySound(SoundManager.Instance.uiClickSound);
		}
	}
}