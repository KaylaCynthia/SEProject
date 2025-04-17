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
    [SerializeField] private float cookingTime = 7f;
    public bool isCooking = false;
    public GameObject burnedFoodPrefab;
    public Transform spawnPoint;

    [SerializeField] private Slider cookingTimerSlider;
    [SerializeField] private GameObject timerUI;
    [Range(0, 7)] [SerializeField] private float minPerfectCook = 5f;
    [Range(0, 7)] [SerializeField] private float maxPerfectCook = 5.5f;

    [SerializeField] private List<string> ingredientsInPot = new List<string>();
    private Inventory inventory;
    private Coroutine cookingCoroutine;
    private float currentCookingTime = 0f;

    //tambahan
    public bool isReady = false;
    [SerializeField] private bool isSaucepan;
    [SerializeField] private stir stirring;
    [SerializeField] private Button back;
    //[SerializeField] private GameObject buttonCook;
    //

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        if (timerUI != null)
        {
            timerUI.SetActive(false);
        }

        //modif
        //stirring.transform.parent.gameObject.SetActive(false);
    }
    //tambahan
    public bool checkIngredientInPot()
    {
        if (ingredientsInPot.Count > 0)
        {
            return true;
        }
        return false;
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
            back.interactable = false;
            if (isSaucepan)
            { 
                stirring.stir_value = 0f;
                stirring.cooked = false;
            }
            //stirring.transform.parent.gameObject.SetActive(true);
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
            isCooking = false;
            ingredientsInPot.Clear();
            timerUI.SetActive(false);
            //tambahan
            back.interactable = true;
            //stirring.transform.parent.gameObject.SetActive(false);
            //buttonCook.SetActive(true);
            //
            StopCoroutine(cookingCoroutine);

            //modif
            //float progress = currentCookingTime / cookingTime;
            float progress = currentCookingTime;
            if (isSaucepan)
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
            else if (!isSaucepan && progress >= minPerfectCook && progress <= maxPerfectCook)
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
        }
    }

    private IEnumerator CookFood()
    {
        //modif + tambahan
        while (!isSaucepan && currentCookingTime < cookingTime)
        {
            if (currentCookingTime >= minPerfectCook && currentCookingTime <= maxPerfectCook)
            {
                isReady = true;
            }
            else
            {
                isReady = false;
            }
            if (!stirring.pointerUp)
            {
                currentCookingTime += Time.deltaTime;
                cookingTimerSlider.value = currentCookingTime;
            }
            yield return null;
        }
        if (!isSaucepan)
        {
            StopCooking();
            yield return null;
        }
        while (isSaucepan && !stirring.cooked)
        {
            if (currentCookingTime < cookingTime)
            {
                currentCookingTime += Time.deltaTime;
                cookingTimerSlider.value = currentCookingTime;
            }
            yield return null;
        }
        if (isSaucepan && stirring.cooked)
        {
            StopCooking();
            yield return null;
        }
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