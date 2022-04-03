using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class BuddyAger : Singleton<BuddyAger>
{
    [SerializeField]
    private List<SpriteRenderer> coloredRenderers = default;

    [SerializeField]
    private List<MonoBehaviour> componentsToDisable = default;

    [SerializeField]
    private BuddyEyes eyes = default;

    [SerializeField]
    private List<Transform> hands = default;

    [SerializeField]
    private Transform body = default;

    [SerializeField]
    private Transform head = default;

    public void Die()
    {
        eyes.SetEmotion(Emotion.Neutral);
        eyes.Close();
        componentsToDisable.ForEach(x => x.enabled = false);

        Tween.LocalPosition(head, new Vector2(head.localPosition.x, 0.06f), 1f, 0f, Tween.EaseOut);
        Tween.LocalPosition(body, new Vector2(body.localPosition.x, -0.16f), 0.5f, 0f, Tween.EaseOut);
        foreach (var hand in hands)
        {
            Tween.LocalPosition(hand, new Vector2(hand.localPosition.x, -0.3f), 0.5f, 0f, Tween.EaseOut);
        }
    }

    public override void ManagedUpdate()
    {
        coloredRenderers.ForEach(x => x.color = GetColor());
    }

    private Color GetColor()
    {
        if (GameClock.Instance.NormalizedTimer < 0.5f)
        {
            return Color.Lerp(BuddyArtRandomizer.bodyColor, Color.gray, GameClock.Instance.NormalizedTimer * 2f);
        }
        else
        {
            return Color.Lerp(Color.gray, Color.white, (GameClock.Instance.NormalizedTimer - 0.5f) * 2f);
        }
    }
}
