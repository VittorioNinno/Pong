///	<summary>
///	A static class to hold persistent data across scenes, like the selected game mode.
///	This class is not a MonoBehaviour and does not get attached to any GameObject.
///	</summary>
public static class GameData
{
	///	<summary>
	///	Defines the possible game modes.
	///	</summary>
	public enum GameMode
	{
		PlayerVsPlayer,
		PlayerVsCpu
	}

	///	<summary>
	///	Stores the currently selected game mode. This variable will persist across scene loads.
	///	</summary>
	public static GameMode currentGameMode;
}