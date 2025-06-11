using UnityEngine;

public class MaterialColorOverride : MonoBehaviour
{
	[Header("Property Override")]
	[Tooltip("The custom color to apply to this object's material.")]
	//	This attribute forces the color picker to show HDR options with an Intensity slider!
	[ColorUsage(true, true)]
	public Color overrideColor = Color.white;

	[Tooltip("The name of the color property in the shader to override.")]
	public string colorPropertyName = "_GlowColor";

	private Renderer objectRenderer;
	private MaterialPropertyBlock propertyBlock;

	///	<summary>
	///	Called when the script instance is being loaded.
	///	</summary>
	private void Awake()
	{
		//	Initialize the components
		objectRenderer = GetComponent<Renderer>();
		propertyBlock = new MaterialPropertyBlock();

		//	Apply the color initially
		ApplyColor();
	}

	///	<summary>
	///	Applies the override color to the material property block.
	///	</summary>
	private void ApplyColor()
	{
		if (objectRenderer == null || propertyBlock == null)
		{
			//	This can happen if called before Awake, e.g. in OnValidate
			return;
		}

		//	Get the current properties from the renderer into our block
		objectRenderer.GetPropertyBlock(propertyBlock);

		//	Set the color property on the block using the property name
		propertyBlock.SetColor(colorPropertyName, overrideColor);

		//	Apply the modified block back to the renderer
		objectRenderer.SetPropertyBlock(propertyBlock);
	}

	///	<summary>
	///	This is called in the editor whenever a value is changed in the Inspector.
	///	It allows us to see the color change live without entering Play Mode.
	///	</summary>
	private void OnValidate()
	{
		//	Ensure components are assigned even if we are not in play mode
		if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();
		if (propertyBlock == null) propertyBlock = new MaterialPropertyBlock();

		ApplyColor();
	}
}