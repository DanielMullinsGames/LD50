using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : Singleton<FoodSpawner>
{
    public bool NoFood => activeFood.Count == 0;

    [SerializeField]
    private UIButton clearFoodButton = default;

    [SerializeField]
    private DialogueEvent clearedFoodEvent = default;

    [SerializeField]
    private List<GameObject> foodPrefabs = default;

    [SerializeField]
    private GameObject bagelPrefab = default;

    [SerializeField]
    private List<DialogueEvent> dropFoodDialogues = new List<DialogueEvent>();

    private List<GameObject> activeFood = new List<GameObject>();

    public void AddObjectToFoodList(GameObject obj)
    {
        activeFood.Add(obj);
    }

    public void UnlockBagel()
    {
        if (!foodPrefabs.Contains(bagelPrefab))
        {
            foodPrefabs.Add(bagelPrefab);
        }
    }

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
        if (activeFood.Count == 250)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[4]);
        }
        if (activeFood.Count == 500)
        {
            DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[5]);
        }
        if (activeFood.Count > 550)
        {
            Application.Quit();
        }
    }

    public void ClearFood()
    {
        GameStatus.pressedClearFoodButton = true;
        activeFood.ForEach(x => Destroy(x));
        activeFood.Clear();
        DialogueHandler.Instance.AddDialogueEventToStack(clearedFoodEvent);
    }
}
