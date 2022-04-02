using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class UIButton : Interactable2D
{
    [SerializeField]
    private SpriteRenderer buttonRenderer = default;

    [SerializeField]
    private Vector2 mouseOverOffset = default;

    protected override void OnCursorEnter()
    {
        Tween.LocalPosition(buttonRenderer.transform, mouseOverOffset, 0.1f, 0f, Tween.EaseOut);
    }

    protected override void OnCursorExit()
    {
        Tween.LocalPosition(buttonRenderer.transform, Vector2.zero, 0.1f, 0f, Tween.EaseIn);
    }

    protected override void OnCursorSelectEnd()
    {
        buttonRenderer.enabled = false;
        CustomCoroutine.WaitThenExecute(0.1f, () => buttonRenderer.enabled = true);
        ScreenEffectsController.Instance.AddThenSubtractIntensity(ScreenEffect.RenderCanvasShake, 0.05f, 0.05f, 0.05f, 0.1f);
    }
}
