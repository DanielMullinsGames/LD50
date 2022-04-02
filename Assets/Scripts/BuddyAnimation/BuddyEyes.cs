using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuddyEyes : TimedBehaviour
{
    [SerializeField]
    private float doubleBlinkChance = 0.2f;

    [SerializeField]
    private List<Sprite> openSprites = default;

    [SerializeField]
    private List<Sprite> closedSprites = default;

    private Emotion currentEmotion = Emotion.Neutral;
    private bool eyesClosed = false;

    public void SetEmotion(Emotion emotion)
    {
        currentEmotion = emotion;
        UpdateSprite();
    }

    protected override void OnTimerReached()
    {
        StartCoroutine(BlinkSequence());
    }

    private IEnumerator BlinkSequence()
    {
        yield return Blink();
        if (Random.value < doubleBlinkChance)
        {
            yield return Blink();
        }
    }

    private IEnumerator Blink()
    {
        eyesClosed = true;
        UpdateSprite();
        yield return new WaitForSeconds(0.1f);
        eyesClosed = false;
        UpdateSprite();
        yield return new WaitForSeconds(0.1f);
    }

    private void UpdateSprite()
    {
        var sprites = eyesClosed ? closedSprites : openSprites;
        GetComponent<SpriteRenderer>().sprite = sprites[(int)currentEmotion - 1];
    }
}
