using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenEffect
{
    RenderCanvasShake,
}

public class ScreenEffectsController : Singleton<ScreenEffectsController>
{
    [SerializeField]
    private JitterPosition renderCanvasShake = default;

    private Dictionary<ScreenEffect, float> effectIntensities = new Dictionary<ScreenEffect, float>();

    public void AddIntensity(ScreenEffect effect, float intensity, float tweenTime)
    {
        effectIntensities.TryAdd(effect, 0f);
        StartCoroutine(TweenEffectIntensity(effect, intensity, tweenTime));
    }

    public void AddThenSubtractIntensity(ScreenEffect effect, float intensity, float tweenInTime, float fullIntensityDuration, float tweenOutTime)
    {
        AddIntensity(effect, intensity, tweenInTime);
        CustomCoroutine.WaitThenExecute(tweenInTime + fullIntensityDuration, () => AddIntensity(effect, -intensity, tweenOutTime));
    }

    public override void ManagedUpdate()
    {
        UpdateEffects();
    }

    private void UpdateEffects()
    {
        foreach (var effectIntensity in effectIntensities)
        {
            float clampedValue = Mathf.Clamp(effectIntensity.Value, 0f, 1f);
            switch (effectIntensity.Key)
            {
                case ScreenEffect.RenderCanvasShake:
                    renderCanvasShake.amount = clampedValue * 100f;
                    break;
            }
        }
    }

    private IEnumerator TweenEffectIntensity(ScreenEffect effect, float intensity, float tweenTime)
    {
        float startTime = Time.time;
        float remainingIntensityToAdd = intensity;

        while (Time.time - tweenTime < startTime)
        {
            float valueToAdd = intensity * (Time.deltaTime / tweenTime);
            remainingIntensityToAdd -= valueToAdd;

            effectIntensities[effect] += valueToAdd;
            yield return new WaitForEndOfFrame();
        }

        effectIntensities[effect] += remainingIntensityToAdd;
    }
}
