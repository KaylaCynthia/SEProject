using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookingPot : MonoBehaviour
{
    [System.Serializable]
    public struct Recipe
    {
        public string dishName;
        public List<string> requiredIngredients;
        public GameObject dishPrefab;
    }

    public List<Recipe> recipe;
    public float cookingTime = 3f;
    private bool isCooking = false;
    public GameObject burnedFoodPrefab;
    public Transform spawnPoint;

    [SerializeField] private List<string> ingredientsInPot = new List<string>();
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void OnTriggerEnter2D(Collider2D ingredient)
    {
        if (ingredient != null && !isCooking)
        {
            food foodItem = ingredient.GetComponent<food>();
            if (foodItem != null)
            {
                ingredientsInPot.Add(foodItem.foodName);
                inventory.RemoveInventory(ingredient.gameObject);
                Destroy(ingredient.gameObject);
                // StartCooking();
            }
        }
    }

    public void StartCooking()
    {
        if (!isCooking && ingredientsInPot.Count > 0)
        {
            StartCoroutine(CookFood());
        }
    }

    private IEnumerator CookFood()
    {
        isCooking = true;
        
        yield return new WaitForSeconds(cookingTime);
        
        Recipe dishToSpawn = IngredientsMatch(ingredientsInPot, recipe);

        if(dishToSpawn.dishPrefab == null)
        {
            inventory.AddIngredient(Instantiate(burnedFoodPrefab, spawnPoint));
        }
        else
        {
            inventory.AddIngredient(Instantiate(dishToSpawn.dishPrefab, spawnPoint));
        }
        
        ingredientsInPot.Clear();
        isCooking = false;
    }

    private Recipe IngredientsMatch(List<string> ingredients, List<Recipe> recipe)
    {
        // foreach(string rIngredients in recipe.requiredIngredients)
        // {
        //     if(ingredients)
        // }
        
        for(int i = 0; i < recipe.Count; i++)
        {
            int count = 0;
            for(int j = 0; j < ingredients.Count; j++)
            {
                if(recipe[i].requiredIngredients.Contains(ingredients[j]))
                {
                    count++;
                    if(count == recipe[i].requiredIngredients.Count)
                    {
                        return recipe[i];
                    }
                }
            }
        }

        return new Recipe();
    }
}