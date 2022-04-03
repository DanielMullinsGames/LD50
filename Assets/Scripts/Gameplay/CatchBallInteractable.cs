using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBallInteractable : Interactable2D
{
    protected override void OnCursorSelectStart()
    {
        AudioController.Instance.PlaySound2D("ball_caught");
        Destroy(transform.parent.gameObject);
        BallThrower.instance.OnBallCaught();
    }
}
