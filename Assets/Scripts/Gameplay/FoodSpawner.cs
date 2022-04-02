using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : ManagedBehaviour
{
    [SerializeField]
    private List<GameObject> foodPrefabs = default;

    [SerializeField]
    private List<DialogueEvent> dropFoodDialogues = new List<DialogueEvent>();

    public void SpawnFood()
    {
        var obj = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Count)]);
        obj.transform.position = transform.position;
        obj.transform.SetParent(transform);

        obj.GetComponent<Rigidbody2D>().AddForce(Vector2.right * CustomRandom.RandomBetween(-50f, 50f));

        DialogueHandler.Instance.AddDialogueEventToStack(dropFoodDialogues[0]);
    }
}
