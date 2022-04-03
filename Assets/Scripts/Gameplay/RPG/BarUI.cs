using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : ManagedBehaviour
{
    [SerializeField]
    private Transform bar = default;

    [SerializeField]
    private float barMaxScale = default;

    private float intendedAmount = 0f;

    public void ShowAmount(float normalized, bool immediate = false)
    {
        intendedAmount = normalized;
        if (immediate)
        {
            bar.transform.localScale = new Vector2(barMaxScale * intendedAmount, bar.transform.localScale.y);
        }
    }

    public override void ManagedUpdate()
    {
        float amount = Mathf.Lerp(bar.transform.localScale.x, intendedAmount, Time.deltaTime);
        bar.transform.localScale = new Vector2(barMaxScale * amount, bar.transform.localScale.y);
    }
}
