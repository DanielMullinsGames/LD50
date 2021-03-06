using UnityEngine;

public abstract class SineWaveTransform : ManagedBehaviour
{
    public float Modifier = 1f;

    public Vector3 magnitude = default;
    public float speed = default;
    public float timeOffset = default;

    private Vector3 originalMagnitude;

    private void Start()
    {
        originalMagnitude = GetMagnitude();
    }

    public override void ManagedUpdate()
    {
        float sine = (Mathf.Sin(Time.time * speed + timeOffset) * 0.5f) + 0.5f;
        float x = originalMagnitude.x + (sine * magnitude.x * Modifier);
        float y = originalMagnitude.y + (sine * magnitude.y * Modifier);
        float z = originalMagnitude.z + (sine * magnitude.z * Modifier);
        ApplyTransformation(new Vector3(x, y, z));
    }

    protected abstract Vector3 GetMagnitude();
    protected abstract void ApplyTransformation(Vector3 value);
}
