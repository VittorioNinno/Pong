using UnityEngine;
using Cysharp.Threading.Tasks;

public class ScreenShake : MonoBehaviour
{
	///	<summary>
	///	Triggers the screen shake effect.
	///	</summary>
	///	<param name="duration">How long the shake should last.</param>
	///	<param name="magnitude">How intense the shake should be.</param>
	///	<returns></returns>
	public async UniTask Shake(float duration, float magnitude)
	{
		Vector3 originalPosition = transform.position;
		float elapsed = 0.0f;

		while(elapsed < duration)
		{
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;

			transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

			elapsed += Time.deltaTime;

			//	Wait for the next frame without memory allocation.
			await UniTask.Yield(PlayerLoopTiming.Update);
		}

		//	Reset the camera position after the shake
		transform.position = originalPosition;
	}
}