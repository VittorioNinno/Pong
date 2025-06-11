using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
	[Header("Audio Clips")]
	[Tooltip("Sound for the ball hitting a paddle.")]
	public AudioClip paddleHitSound;
	[Tooltip("Sound for the ball hitting a wall.")]
	public AudioClip wallHitSound;
	[Tooltip("Sound for a player scoring a goal.")]
	public AudioClip goalSound;
	[Tooltip("Sound for clicking a UI button.")]
	public AudioClip uiClickSound;

	//	--- Singleton Instance ---
	public static SoundManager Instance { get; private set; }

	private AudioSource sfxSource;

	///	<summary>
	///	Initializes the Singleton instance and gets the AudioSource component.
	///	</summary>
	private void Awake()
	{
		//	Singleton Setup
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		sfxSource = GetComponent<AudioSource>();
	}

	///	<summary>
	///	Plays a given audio clip as a one-shot sound.
	///	</summary>
	///	<param name="clip">The AudioClip to play.</param>
	public void PlaySound(AudioClip clip)
	{
		if (clip != null)
		{
			//	PlayOneShot allows playing multiple sounds without interrupting each other.
			sfxSource.PlayOneShot(clip);
		}
	}
}