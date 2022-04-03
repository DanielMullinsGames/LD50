using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClock : Singleton<GameClock>
{
    public float NormalizedTimer => Timer / MAX_TIME;
    public int Hours => Mathf.FloorToInt(Minutes / 60f);
    public int Minutes => Mathf.RoundToInt(Timer / 60f);
    public float Timer { get; private set; }

    public const float MAX_TIME = 48f * 60f * 60f;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public override void ManagedUpdate()
    {
        Timer += Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.H))
        {
            Timer += 60f * 60f;
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
        {
            Timer += 60f;
        }
    }
}
