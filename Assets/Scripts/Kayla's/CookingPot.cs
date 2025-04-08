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

    [SerializeField] private Slider cookingTimerSlider;
    [SerializeField] private GameObject timerUI;
    [Range(0, 1)] public float minPerfectCook = 0.5f;
    [Range(0, 1)] public float maxPerfectCook = 0.7f;

    [SerializeField] private List<string> ingredientsInPot = new List<string>();
    private Inventory inventory;
    private Coroutine cookingCoroutine;
    private float currentCookingTime = 0f;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        if (timerUI != null)
        {
            timerUI.SetActive(false);
        }
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

    public void ToggleCooking()
    {
        if (ingredientsInPot.Count == 0) return;
        
        if (!isCooking)
        {
            StartCooking();
        }
        else
        {
            StopCooking();
        }
    }

    public void StartCooking()
    {
        if (!isCooking && ingredientsInPot.Count > 0)
        {
            isCooking = true;
            currentCookingTime = 0f;
            timerUI.SetActive(true);
            cookingTimerSlider.value = 0f;
            cookingCoroutine = StartCoroutine(CookFood());
        }
    }

    public void StopCooking()
    {
        if (isCooking)
        {
            StopCoroutine(cookingCoroutine);
            float progress = currentCookingTime / cookingTime;

            if (progress >= minPerfectCook && progress <= maxPerfectCook)
            {
                Recipe dishToSpawn = IngredientsMatch(ingredientsInPot, recipe);
                if (dishToSpawn.dishPrefab != null)
                {
                    inventory.AddIngredient(Instantiate(dishToSpawn.dishPrefab, spawnPoint));
                }
                else
                {
                    inventory.AddIngredient(Instantiate(burnedFoodPrefab, spawnPoint));
                }
            }
            else
            {
                inventory.AddIngredient(Instantiate(burnedFoodPrefab, spawnPoint));
            }

            ingredientsInPot.Clear();
            isCooking = false;
            timerUI.SetActive(false);
        }
    }

    private IEnumerator CookFood()
    {
        while (currentCookingTime < cookingTime)
        {
            currentCookingTime += Time.deltaTime;
            cookingTimerSlider.value = currentCookingTime / cookingTime;
            yield return null;
        }

        inventory.AddIngredient(Instantiate(burnedFoodPrefab, spawnPoint));
        ingredientsInPot.Clear();
        isCooking = false;
        timerUI.SetActive(false);
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