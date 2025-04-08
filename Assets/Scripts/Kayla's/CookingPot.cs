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
    [Range(0, 1)] public float minPerfectCook = 1.5f;
    [Range(0, 1)] public float maxPerfectCook = 2f;

    [SerializeField] private List<string> ingredientsInPot = new List<string>();
    private Inventory inventory;
    private Coroutine cookingCoroutine;
    private float currentCookingTime = 0f;

    //tambahan
    [SerializeField] private stir stirring;
    //[SerializeField] private GameObject buttonCook;
    //

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        if (timerUI != null)
        {
            timerUI.SetActive(false);
        }
        stirring.transform.parent.gameObject.SetActive(false);
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
            //tambahan
            stirring.stir_value = 0f;
            stirring.transform.parent.gameObject.SetActive(true);
            //buttonCook.SetActive(false);
            //
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
            //tambahan
            stirring.transform.parent.gameObject.SetActive(false);
            //buttonCook.SetActive(true);
            //
            StopCoroutine(cookingCoroutine);
            float progress = currentCookingTime / cookingTime;

            
            if (progress >= minPerfectCook && progress <= maxPerfectCook && stirring.cooked)
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

            //tambahan
            stirring.cooked = false;
            //
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
            //modif
            cookingTimerSlider.value = cookingTime - currentCookingTime;
            yield return null;
        }
        //modif + tambahan
        StopCooking();
        /*inventory.AddIngredient(Instantiate(burnedFoodPrefab, spawnPoint));
        ingredientsInPot.Clear();
        isCooking = false;
        timerUI.SetActive(false);*/
        //
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