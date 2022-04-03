using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoCodeWindow : ManagedBehaviour
{
    public bool Active => window.activeInHierarchy;

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
                EvaluateInput();
                window.SetActive(false);
            }

            UpdateKeyboardInput();
            inputText.text = input;
        }
    }

    private void EvaluateInput()
    {
        if (input.ToLower() == "hatbuddy")
        {
            DeathHandler.Instance.AddHat();
            PlaySuccessSound();
        }
        else if (input.ToLower() == "lionel.exe")
        {
            PlaySuccessSound();
        }
        else if (input.ToLower() == "ponies")
        {
            PlaySuccessSound();
        }
        else if (input.ToLower() == "holiday" && GameStatus.canUseHolidayCode)
        {
            PlaySuccessSound();
            GameStatus.usedHolidayCode = true;
        }
        else if (input.ToLower() == "swordbuddy" && GameStatus.isRPG)
        {
            PlaySuccessSound();
            RPGManager.Instance.SwordPromoCode();
        }
        else
        {
            AudioController.Instance.PlaySound2D("promo_fail");
        }
    }

    private void PlaySuccessSound()
    {
        AudioController.Instance.PlaySound2D("promo_success");
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
