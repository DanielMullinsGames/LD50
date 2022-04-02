using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuddyMouth : ManagedBehaviour
{
    [SerializeField]
    private List<Sprite> openSprites = default;

    [SerializeField]
    private List<Sprite> closedSprites = default;

    private Emotion currentEmotion = Emotion.Neutral;
    private bool mouthClosed = false;

    public void SetOpen(bool open)
    {
        mouthClosed = !open;
        UpdateSprite();
    }

    public void SetEmotion(Emotion emotion)
    {
        currentEmotion = emotion;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        var sprites = mouthClosed ? closedSprites : openSprites;
        GetComponent<SpriteRenderer>().sprite = sprites[(int)currentEmotion - 1];
    }
}
