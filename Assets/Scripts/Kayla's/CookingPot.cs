using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
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
    [Range(0, 7)] public float minPerfectCook = 5f;
    [Range(0, 7)] public float maxPerfectCook = 5.5f;

    private List<string> ingredientsInPot = new List<string>();
    private Inventory inventory;
    private Coroutine cookingCoroutine;
    private float currentCookingTime = 0f;

    //tambahan
    public bool isReady = false;
    [SerializeField] private List<GameObject> foodPosInPot;
    order_notes notes;
    [SerializeField] private bool isSaucepan;
    [SerializeField] private stir stirring;
    [SerializeField] private Button back;
    //[SerializeField] private GameObject buttonCook;

    private void Start()
    {
        notes = FindObjectOfType<order_notes>();
        inventory = FindObjectOfType<Inventory>();
        if (timerUI != null)
        {
            timerUI.SetActive(false);
        }

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            foodPosInPot.Add(transform.GetChild(1).GetChild(i).gameObject);
            transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        //stirring.transform.parent.gameObject.SetActive(false);
    }
    
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
        if (ingredient != null && !isCooking && ingredientsInPot.Count < foodPosInPot.Count && ingredient.gameObject.name != "SoupSet(Clone)" && ingredient.gameObject.name != "StirFrySet(Clone)")
        {
            food foodItem = ingredient.GetComponent<food>();
            if (foodItem != null)
            {
                ingredientsInPot.Add(foodItem.foodName);
                inventory.RemoveInventory(ingredient.gameObject);

                //tambahan, biar ingredientsnya keliatan di dlm pot
                if (!isSaucepan && FindObjectOfType<Customer>().orderName.Contains("Stir Fry"))
                {
                    notes.check(2);
                }
                foreach (GameObject food in foodPosInPot)
                {
                    if (food.GetComponent<Image>().sprite == null || !food.activeSelf)
                    {
                        if(foodItem.foodName != "Egg") food.GetComponent<Image>().sprite = ingredient.GetComponent<Image>().sprite;
                        else food.GetComponent<Image>().sprite = foodItem.eggSprite;
                        food.GetComponent<Image>().color = ingredient.GetComponent<Image>().color;
                        food.SetActive(true);
                        break;
                    }
                }
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
        if (!isCooking && ingredientsInPot.Count > 0 /*tambahan biar limit jumlah ingredients sesuai jumlah foodPosInPot*/ && ingredientsInPot.Count <= foodPosInPot.Count)
        {
            //tambahan
            //back.interactable = false;
            back.gameObject.SetActive(false);
            stirring.stir_value = 0f;
            stirring.cooked = false;
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

    //tambahan buat Invoke
    void goBack()
    { 
        FindObjectOfType<Kitchen_Switch>().backToMain(transform.parent.GetComponent<RectTransform>());
        back.gameObject.SetActive(true);
    }
    public void StopCooking()
    {
        if (isCooking)
        {
            isCooking = false;

            //tambahan
            stirring.inventory.GetComponent<Animator>().SetBool("up", false);
            Invoke("goBack", 1f);
            foreach (GameObject food in foodPosInPot)
            {
                food.SetActive(false);
                food.GetComponent<Image>().sprite = null;
            }
            //back.interactable = true;
            stirring.stir_value = 0f;
            //stirring.transform.parent.gameObject.SetActive(false);
            //buttonCook.SetActive(true);
            //
            if(cookingCoroutine != null) StopCoroutine(cookingCoroutine);

            //modif
            //float progress = currentCookingTime / cookingTime;
            float progress = currentCookingTime;
            if (isSaucepan)
            {
                Recipe dishToSpawn = IngredientsMatch(ingredientsInPot, recipe);
                if (dishToSpawn.dishPrefab != null)
                {
                    GameObject dish = Instantiate(dishToSpawn.dishPrefab, spawnPoint);
                    Vector2 size = GameObject.Find("Slot1").transform.GetChild(0).GetComponent<RectTransform>().rect.size;
                    dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                    dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                    dish.GetComponent<RectTransform>().localScale = Vector3.one;
                    inventory.AddIngredient(dish);
                }
                else
                {
                    GameObject dish = Instantiate(burnedFoodPrefab, spawnPoint);
                    Vector2 size = GameObject.Find("Slot1").transform.GetChild(0).GetComponent<RectTransform>().rect.size;
                    dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                    dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                    dish.GetComponent<RectTransform>().localScale = Vector3.one;
                    inventory.AddIngredient(dish);
                }
                boilingAnimation anim = GameObject.FindObjectOfType<boilingAnimation>();
                anim.doneCooking();
            }
            else if (!isSaucepan && progress >= minPerfectCook && progress <= maxPerfectCook)
            {
                if (FindObjectOfType<Customer>().orderName.Contains("Stir Fry"))
                {
                    notes.check(3);
                    notes.check(4);
                }
                else if (FindObjectOfType<Customer>().orderName.Contains("Soup Set"))
                {
                    notes.check(2);
                    notes.check(3);
                }
                Recipe dishToSpawn = IngredientsMatch(ingredientsInPot, recipe);
                if (dishToSpawn.dishPrefab != null)
                {
                    GameObject dish = Instantiate(dishToSpawn.dishPrefab, spawnPoint);
                    Vector2 size = GameObject.Find("Slot1").transform.GetChild(0).GetComponent<RectTransform>().rect.size;
                    dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                    dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                    dish.GetComponent<RectTransform>().localScale = Vector3.one;
                    inventory.AddIngredient(dish);
                }
                else
                {
                    GameObject dish = Instantiate(burnedFoodPrefab, spawnPoint);
                    Vector2 size = GameObject.Find("Slot1").transform.GetChild(0).GetComponent<RectTransform>().rect.size;
                    dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                    dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                    dish.GetComponent<RectTransform>().localScale = Vector3.one;
                    inventory.AddIngredient(dish);
                }
            }
            else
            {
                //inventory.AddIngredient(Instantiate(burnedFoodPrefab, spawnPoint));
                GameObject dish = Instantiate(burnedFoodPrefab, spawnPoint);
                Vector2 size = GameObject.Find("Slot1").transform.GetChild(0).GetComponent<RectTransform>().rect.size;
                dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                dish.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                dish.GetComponent<RectTransform>().localScale = Vector3.one;
                inventory.AddIngredient(dish);
            }
            ingredientsInPot.Clear();
            timerUI.SetActive(false);
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
        while (isSaucepan && currentCookingTime < cookingTime || isSaucepan && stirring.stir_value < 100)
        {
            if (currentCookingTime < cookingTime)
            {
                currentCookingTime += Time.deltaTime;
                cookingTimerSlider.value = currentCookingTime;
            }
            yield return null;
        }
        if (isSaucepan && stirring.stir_value >= 100)
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