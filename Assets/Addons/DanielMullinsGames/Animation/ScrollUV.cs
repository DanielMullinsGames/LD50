using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class ScrollUV : MonoBehaviour
{

	[SerializeField]
	private Vector2 startOffset = default;

    [SerializeField]
    private int materialIndex = 0;

	public Vector2 scrollVector;
	public float scrollSpeed;

    private Renderer mainRenderer;

	void Start()
	{
        mainRenderer = GetComponent<Renderer>();

        if (HasProperty())
        {
            mainRenderer.materials[materialIndex].mainTextureOffset = startOffset;
        }
        else
        {
            enabled = false;
        }
	}

	void Update()
    {
        if (HasProperty())
        {
            mainRenderer.materials[materialIndex].mainTextureOffset += scrollVector * scrollSpeed * Time.deltaTime;
        }
	}

    private bool HasProperty()
    {
        return mainRenderer.materials[materialIndex].HasProperty("_MainTex");
    }
}
