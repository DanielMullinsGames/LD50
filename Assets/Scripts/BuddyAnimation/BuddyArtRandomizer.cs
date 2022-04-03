using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyArtRandomizer : ManagedBehaviour
{
    public static Color bodyColor = default;

    [SerializeField]
    private SpriteRenderer headRenderer = default;

    [SerializeField]
    private SpriteRenderer bodyRenderer = default;

    [SerializeField]
    private List<SpriteRenderer> coloredRenderers = default;

    [SerializeField]
    private List<Sprite> headSprites = default;

    [SerializeField]
    private List<Sprite> bodySprites = default;

    [SerializeField]
    private List<Color> spriteColors = default;

    private void Start()
    {
        Randomize();
    }

#if UNITY_EDITOR
    public override void ManagedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Randomize();
        }
    }
#endif

    private void Randomize()
    {
        headRenderer.sprite = headSprites[Random.Range(0, headSprites.Count)];
        bodyRenderer.sprite = bodySprites[Random.Range(0, bodySprites.Count)];
        bodyColor = spriteColors[Random.Range(0, spriteColors.Count)];
        coloredRenderers.ForEach(x => x.color = bodyColor);
    }
}
