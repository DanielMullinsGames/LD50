using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyHandsController : Singleton<BuddyHandsController>
{
    [SerializeField]
    private SineWaveMovement leftHandIdleMovement = default;

    [SerializeField]
    private SineWaveMovement rightHandIdleMovement = default;

    [SerializeField]
    private MatchPosition leftHandMatchPos = default;

    [SerializeField]
    private MatchPosition rightHandMatchPos = default;

    public void ClearHandTargets()
    {
        leftHandMatchPos.enabled = rightHandMatchPos.enabled = false;
        leftHandIdleMovement.enabled = rightHandIdleMovement.enabled = true;
    }

    public void SetLeftHandTarget(Transform target)
    {
        leftHandMatchPos.enabled = true;
        leftHandMatchPos.target = target;
        leftHandIdleMovement.enabled = false;
    }

    public void SetRightHandTarget(Transform target)
    {
        rightHandMatchPos.enabled = true;
        rightHandMatchPos.target = target;
        rightHandIdleMovement.enabled = false;
    }
}
