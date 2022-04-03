using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallThrower : TimedBehaviour
{
    public static BallThrower instance;

    [SerializeField]
    private Animator throwerAnim = default;

    [SerializeField]
    private GameObject ballObj = default;

    [SerializeField]
    private Transform handTarget = default;

    [SerializeField]
    private Text ballsText = default;

    [SerializeField]
    private List<DialogueEvent> caughtEvents = default;

    [SerializeField]
    private List<DialogueEvent> droppedEvents = default;

    public int BallsCaught { get; private set; }

    private int droppedIndex = 0;
    private bool playDroppedDialogue = false;

    private void Start()
    {
        instance = this;
    }

    public void OnBallLanded()
    {
        if (playDroppedDialogue && BallsCaught < 3)
        {
            if (droppedEvents.Count > droppedIndex)
            {
                DialogueHandler.Instance.AddDialogueEventToStack(droppedEvents[droppedIndex]);
                droppedIndex++;
            }
        }
        playDroppedDialogue = !playDroppedDialogue;
    }

    public void OnBallCaught()
    {
        BallsCaught++;
        ballsText.text = "Balls Caught: " + BallsCaught.ToString();

        switch (BallsCaught)
        {
            case 1:
                DialogueHandler.Instance.AddDialogueEventToStack(caughtEvents[0]);
                break;
            case 2:
                DialogueHandler.Instance.AddDialogueEventToStack(caughtEvents[1]);
                break;
            case 5:
                DialogueHandler.Instance.AddDialogueEventToStack(caughtEvents[2]);
                break;
            case 8:
                DialogueHandler.Instance.AddDialogueEventToStack(caughtEvents[3]);
                break;
        }
    }

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
        FoodSpawner.Instance.AddObjectToFoodList(newBall);
    }
}
