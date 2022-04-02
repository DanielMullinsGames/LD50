using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDisplayer : ManagedBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = default;

    [SerializeField]
    private List<Sprite> cursorTextures = default;

    [SerializeField]
    private List<Sprite> cursorDownTextures = default;

    public void SetCursorDown(bool down, CursorType type)
    {
        int typeIndex = Mathf.Min(cursorTextures.Count - 1, (int)type);
        spriteRenderer.sprite = down ? cursorDownTextures[typeIndex] : cursorTextures[typeIndex];
    }
}
