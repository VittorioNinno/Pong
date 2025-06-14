using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[Tooltip("Drag the Scene Asset you want to load here.")]
	public Object sceneToLoad;

	///	<summary>
	///	Loads the scene assigned in the 'sceneToLoad' field.
	///	This method is designed to be called from UI events like a button's OnClick.
	///	</summary>
	public void LoadScene()
	{
		if (sceneToLoad != null)
		{
			//	Usiamo .name per ottenere la stringa del nome della scena dall'asset
			SceneManager.LoadScene(sceneToLoad.name);
		}
		else
		{
			Debug.LogError("Scene to load is not assigned in the SceneLoader component!", this.gameObject);
		}
	}
}