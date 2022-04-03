using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : ManagedBehaviour
{
    [SerializeField]
    private Transform bar = default;

    [SerializeField]
    private float barMaxScale = default;

    private float currentAmount = 0f;
    private float intendedAmount = 0f;

    public void ShowAmount(float normalized, bool immediate = false)
    {
        intendedAmount = normalized;
        if (immediate)
        {
            currentAmount = intendedAmount;
            bar.transform.localScale = new Vector2(barMaxScale * intendedAmount, bar.transform.localScale.y);
        }
    }

    public override void ManagedUpdate()
    {
        currentAmount = Mathf.Lerp(currentAmount, intendedAmount, Time.deltaTime * 10f);
        currentAmount = Mathf.Clamp(currentAmount, 0f, 1f);
        bar.transform.localScale = new Vector2(barMaxScale * currentAmount, bar.transform.localScale.y);
    }
}
