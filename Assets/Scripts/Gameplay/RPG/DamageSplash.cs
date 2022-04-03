using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSplash : ManagedBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text text = default;

    [SerializeField]
    private GameObject critObj = default;

    public void UpdateDisplay(int damage, bool crit)
    {
        text.text = damage.ToString();
        critObj.SetActive(crit);
    }
}
