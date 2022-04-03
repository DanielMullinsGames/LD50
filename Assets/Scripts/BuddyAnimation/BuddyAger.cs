using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyAger : ManagedBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> coloredRenderers = default;

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
