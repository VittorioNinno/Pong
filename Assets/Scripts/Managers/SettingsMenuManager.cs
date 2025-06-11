using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
	[Header("Components")]
	[Tooltip("The main audio mixer for the game.")]
	public AudioMixer mainMixer;
	[Tooltip("The UI slider that controls the volume.")]
	public Slider volumeSlider;

	private const string VOLUME_PREF_KEY = "MasterVolume";

	///	<summary>
	///	Called when the settings panel becomes active. Loads saved settings.
	///	</summary>
	private void Start()
	{
		//	Load the saved volume preference and set the slider's initial value.
		float savedVolume = PlayerPrefs.GetFloat(VOLUME_PREF_KEY, 1f);
		volumeSlider.value = savedVolume;

		SetVolume(savedVolume);
	}

	///	<summary>
	///	Called by the VolumeSlider's OnValueChanged event.
	///	</summary>
	///	<param name="linearValue">The slider's value, from 0.0001 to 1.</param>
	public void SetVolume(float linearValue)
	{
		float dbValue = Mathf.Log10(linearValue) * 20;
		mainMixer.SetFloat("MasterVolume", dbValue);

		//	Save the player's preference so it persists between game sessions.
		PlayerPrefs.SetFloat(VOLUME_PREF_KEY, linearValue);
	}
}