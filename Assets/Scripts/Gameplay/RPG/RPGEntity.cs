using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGEntity : ManagedBehaviour
{
    [SerializeField]
    private BarUI healthBar = default;

    public bool Alive => health > 0;

    public int maxHealth;
    public int health;
    public int attackPower;
    public float critChance;
    public int critMultiplier = 2;

    public IEnumerator PlayAttackAnim()
    {
        //anim
        yield return new WaitForSeconds(0.1f);

    }

    public void UpdateHealthBar()
    {

    }

    public void TakeHitAnim()
    {
        ScreenEffectsController.Instance.AddThenSubtractIntensity(ScreenEffect.RenderCanvasShake, 0.05f, 0.05f, 0.05f, 0.1f);
        AudioController.Instance.PlaySound2D("button_press_chunk", 1f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
    }
}
