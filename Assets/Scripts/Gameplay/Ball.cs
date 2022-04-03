using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : ManagedBehaviour
{
    [SerializeField]
    private CatchBallInteractable catchBallInteractable = default;

    private bool landed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.name == "Floor" || collision.transform.GetComponent<Ball>())
        {
            OnLanded();
        }
    }

    private void OnLanded()
    {
        if (!landed)
        {
            AudioController.Instance.PlaySound2D("ball_land", volume: 0.5f);
            catchBallInteractable.SetCollisionEnabled(false);
            BallThrower.instance.OnBallLanded();
            landed = true;
        }
    }
}
