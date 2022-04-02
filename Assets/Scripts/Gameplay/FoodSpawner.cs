using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : ManagedBehaviour
{
    [SerializeField]
    private UIButton clearFoodButton = default;

    [SerializeField]
    private DialogueEvent clearedFoodEvent = default;

    [SerializeField]
    private List<GameObject> foodPrefabs = default;

    [SerializeField]
    private List<DialogueEvent> dropFoodDialogues = new List<DialogueEvent>();

    private List<GameObject> activeFood = new List<GameObject>();

    public void SpawnFood()
    {
        var obj = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Count)]);
        obj.transform.position = transform.position;
        obj.transform.SetParent(transform);
        activeFood.Add(obj);

        obj.GetComponent<Rigidbody2D>().AddForce(Vector2.right * CustomRandom.RandomBetween(-50f, 50f));

        if (activeFood.Count == 3)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[0]);
        }
        if (activeFood.Count == 20)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[1]);
        }
        if (activeFood.Count == 50)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[2]);

            if (!DeathHandler.Instance.Dead)
            {
                CustomCoroutine.WaitOnConditionThenExecute(() => DialogueHandler.Instance.CurrentEvent != dropFoodDialogues[2],
                    () => clearFoodButton.Show());
            }
        }
        if (activeFood.Count == 100)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[3]);
        }
        if (activeFood.Count == 300)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[4]);
        }
        if (activeFood.Count == 1000)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[5]);
        }
        if (activeFood.Count > 1100)
        {
            Application.Quit();
        }
    }

    public void ClearFood()
    {
        activeFood.ForEach(x => Destroy(x));
        DialogueHandler.Instance.AddDialogueEventToStack(clearedFoodEvent);
    }
}
