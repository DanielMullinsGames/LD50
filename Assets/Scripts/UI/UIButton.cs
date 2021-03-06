using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.Events;

public class UIButton : Interactable2D
{
    [SerializeField]
    private UnityEvent onSelectEndEvent = default;

    [SerializeField]
    private SpriteRenderer buttonRenderer = default;

    [SerializeField]
    private Vector2 mouseOverOffset = default;

    [SerializeField]
    private bool startHidden = false;

    [SerializeField]
    private bool playEffects = true;

    private void Start()
    {
        if (startHidden)
        {
            SetHidden();
        }
    }

    public void SetHidden()
    {
        buttonRenderer.transform.localPosition = new Vector2(mouseOverOffset.x * -11f, 0f);
        SetCollisionEnabled(false);
    }

    public void Show()
    {
        Tween.LocalPosition(buttonRenderer.transform, Vector2.zero, 0.15f, 0f, Tween.EaseIn);
        AudioController.Instance.PlaySound2D("button_mouseover", 0.5f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small), 
            repetition: new AudioParams.Repetition(0.02f));
        SetCollisionEnabled(true);
    }

    protected override void OnCursorEnter()
    {
        Tween.LocalPosition(buttonRenderer.transform, mouseOverOffset, 0.1f, 0f, Tween.EaseOut);
        AudioController.Instance.PlaySound2D("button_mouseover", 0.5f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small),
            repetition: new AudioParams.Repetition(0.02f));
    }

    protected override void OnCursorExit()
    {
        Tween.LocalPosition(buttonRenderer.transform, Vector2.zero, 0.1f, 0f, Tween.EaseIn);
    }

    protected override void OnCursorSelectEnd()
    {
        if (playEffects)
        {
            buttonRenderer.enabled = false;
            CustomCoroutine.WaitThenExecute(0.1f, () => buttonRenderer.enabled = true);
            ScreenEffectsController.Instance.AddThenSubtractIntensity(ScreenEffect.RenderCanvasShake, 0.05f, 0.05f, 0.05f, 0.1f);
            AudioController.Instance.PlaySound2D("button_press_chunk", 1f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
        }

        onSelectEndEvent?.Invoke();
    }
}
