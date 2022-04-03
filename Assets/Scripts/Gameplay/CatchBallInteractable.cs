using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBallInteractable : Interactable2D
{
    [SerializeField]
    private Ball ball = default;

    private void Start()
    {
        transform.parent = null;
    }

    protected override void OnCursorSelectStart()
    {
        AudioController.Instance.PlaySound2D("ball_caught");
        Destroy(ball.gameObject);
        Destroy(gameObject);
        BallThrower.instance.OnBallCaught();
    }
}
