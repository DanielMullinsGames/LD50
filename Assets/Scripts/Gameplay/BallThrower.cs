using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : TimedBehaviour
{
    [SerializeField]
    private Animator throwerAnim = default;

    [SerializeField]
    private GameObject ballObj = default;

    [SerializeField]
    private Transform handTarget = default;

    protected override void OnTimerReached()
    {
        StartThrow();
    }

    private void StartThrow()
    {
        BuddyHandsController.Instance.SetLeftHandTarget(handTarget);
        throwerAnim.Play("throw", 0, 0f);
    }

    public void OnReleaseKeyframe()
    {
        var newBall = Instantiate(ballObj);
        newBall.SetActive(true);
        newBall.transform.position = ballObj.transform.position;
        newBall.GetComponent<Rigidbody2D>().AddForce(Vector2.up * CustomRandom.RandomBetween(150f, 200f) + Vector2.right * CustomRandom.RandomBetween(-5f, 100f));
    }
}
