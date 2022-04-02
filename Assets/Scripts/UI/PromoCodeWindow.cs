using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoCodeWindow : ManagedBehaviour
{
    [SerializeField]
    private GameObject window = default;

    [SerializeField]
    private UnityEngine.UI.Text inputText = default;

    [SerializeField]
    private int maxInputLength = 0;

    private string input = "";

    public void OnPromoCodePressed()
    {
        window.SetActive(true);
        input = "";
    }

    public override void ManagedUpdate()
    {
        if (window.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                window.SetActive(false);
            }

            UpdateKeyboardInput();
            inputText.text = input;
        }
    }

    private void UpdateKeyboardInput()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (input.Length != 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    AudioController.Instance.PlaySound2D("button_press", 0.25f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
                }
            }
            else
            {
                if (input.Length < maxInputLength)
                {
                    input += c.ToString().ToUpper();
                    AudioController.Instance.PlaySound2D("button_press_chunk", 0.25f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
                    ScreenEffectsController.Instance.AddThenSubtractIntensity(ScreenEffect.RenderCanvasShake, 0.02f, 0.05f, 0.05f, 0.1f);
                }
            }
        }
    }
}
