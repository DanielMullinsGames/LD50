using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGEntity : ManagedBehaviour
{
    [SerializeField]
    private BarUI healthBar = default;

    [SerializeField]
    private Animator anim = default;

    public bool Alive => health > 0;

    public int maxHealth;
    public int health;
    public int attackPower;
    public float critChance;
    public int critMultiplier = 2;

    private void Start()
    {
        UpdateHealthBar(true);
    }

    public IEnumerator PlayAttackAnim()
    {
        if (anim != null)
        {
            anim.Play("attack", 0, 0f);
        }
        yield return new WaitForSeconds(0.25f);

    }

    public void UpdateHealthBar(bool immediate = false)
    {
        healthBar.ShowAmount(health / (float)maxHealth, immediate);
    }

    public void TakeHitAnim()
    {
        if (anim != null)
        {
            anim.Play("hit", 0, 0f);
        }

        ScreenEffectsController.Instance.AddThenSubtractIntensity(ScreenEffect.RenderCanvasShake, 0.05f, 0.05f, 0.05f, 0.1f);
        AudioController.Instance.PlaySound2D("button_press_chunk", 1f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
    }
}
