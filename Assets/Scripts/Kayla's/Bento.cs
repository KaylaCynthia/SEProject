using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bento : MonoBehaviour
{
    [System.Serializable]
    public struct BentoSlot
    {
        public Image slotImage;
        public List<string> allowedFood;
        public List<Sprite> allowedFoodImage;
        public bool isOccupied;
    }
    [SerializeField] private Canvas customerCanvas;

    public string bentoName;
    private Inventory inventory;
    private switch_room switch_Room;
    public List<string> ingredientsInBento = new List<string>();
    public List<BentoSlot> bentoSlots;
    public GameObject bentoToServePrefab;
    public Transform servePosition;
    private int targetSlotImage;
    private Kitchen_Switch kitchen_Switch;
    [SerializeField] private RectTransform bento;
    TutorialManager tutor;
    [SerializeField] private AudioSource sfx;
    private void Start()
    {
        tutor = FindObjectOfType<TutorialManager>();
        switch_Room = FindObjectOfType<switch_room>();
        inventory = FindObjectOfType<Inventory>();
        kitchen_Switch = FindObjectOfType<Kitchen_Switch>();
        // bentoSlots = new List<BentoSlot>(4);
        
        for (int i = 0; i < bentoSlots.Count; i++)
        {
            string slotName = "Bento" + (i + 1);
            Transform slotTransform = transform.Find(slotName);
            
            if (slotTransform != null)
            {
                BentoSlot slot = bentoSlots[i];
                slot.slotImage = slotTransform.GetComponent<Image>();
                slot.slotImage.enabled = false;
                bentoSlots[i] = slot;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D ingredient)
    {
        if (ingredient == null) return;

        food foodItem = ingredient.GetComponent<food>();
        if (foodItem == null) return;

        int targetSlot = FindValidSlotForIngredient(foodItem.foodName);
        Debug.Log(targetSlot);
        if (targetSlot == -1)
        {
            Debug.Log("No valid slot for: " + foodItem.foodName);
            return;
        }

        BentoSlot slot = bentoSlots[targetSlot];
        ingredientsInBento.Add(foodItem.foodName);
        slot.slotImage.sprite = slot.allowedFoodImage[targetSlotImage];
        slot.slotImage.enabled = true;
        slot.isOccupied = true;
        
        bentoSlots[targetSlot] = slot;
        
        inventory.RemoveInventory(ingredient.gameObject);
        Destroy(ingredient.gameObject);

        if(CheckIfFull())
        {
            if(ingredientsInBento.Contains("StirFrySet") && ingredientsInBento.Contains("ChildMilk") && ingredientsInBento.Contains("Orange"))
            {
                bentoName = "ChildStirFryBento";
            }
            else if(ingredientsInBento.Contains("StirFrySet") && ingredientsInBento.Contains("MomMilk") && ingredientsInBento.Contains("Orange"))
            {
                bentoName = "MomStirFryBento";
            }
            else if(ingredientsInBento.Contains("SoupSet") && ingredientsInBento.Contains("ChildMilk") && ingredientsInBento.Contains("Banana"))
            {
                bentoName = "ChildSoupBento";
            }
            else if(ingredientsInBento.Contains("SoupSet") && ingredientsInBento.Contains("MomMilk") && ingredientsInBento.Contains("Banana"))
            {
                bentoName = "MomSoupBento";
            }
            else
            {
                bentoName = "FAILED";
            }

            ServeCompletedBento(bentoName);
            //modif
            //kitchen_Switch.openBento(bento);
            GetComponent<Animator>().Play("closed");
            kitchen_Switch.hideBento(true);
            //
            //switch_Room.switchRoom(customerCanvas);
        }
    }

    //tambahan buat UI/UX
    public void sound()
    {
        sfx.Play();
    }
    public void backToCustomer()
    {
        if (tutor.isTutoring && tutor.idx == 18)
        {
            tutor.nextTut();
        }
        CuttingBoard cut = FindObjectOfType<CuttingBoard>();
        cut.resetVar();
        switch_Room.switchRoom(customerCanvas);
        kitchen_Switch.hideBento(false);
        kitchen_Switch.openBento(bento);
        ClearBento();
    }
    //

    private void ServeCompletedBento(string bentoName)
    {
        if (bentoToServePrefab != null && servePosition != null)
        {
            GameObject servedBento = Instantiate(bentoToServePrefab, servePosition);
            servedBento.GetComponent<RectTransform>().localPosition = Vector3.zero;
            servedBento.name = bentoName;
        }
        else
        {
            Debug.LogError("BentoToServe prefab or serve position not assigned!");
        }
    }


    private int FindValidSlotForIngredient(string foodName)
    {
        foodName = foodName.Replace("(Clone)", "");
        for (int i = 0; i < bentoSlots.Count; i++)
        {
            if (bentoSlots[i].allowedFood.Contains(foodName) && !bentoSlots[i].isOccupied)
            {
                targetSlotImage = bentoSlots[i].allowedFood.IndexOf(foodName);
                return i;
            }
        }
        return -1;
    }

    private void ClearBento()
    {
        for (int i = 0; i < bentoSlots.Count; i++)
        {
            BentoSlot slot = bentoSlots[i];
            slot.slotImage.enabled = false;
            slot.isOccupied = false;
            bentoSlots[i] = slot;
        }
        ingredientsInBento.Clear();
        bentoName = "";
    }

    private bool CheckIfFull()
    {
        for (int i = 0; i < bentoSlots.Count; i++)
        {
            if (!bentoSlots[i].isOccupied)
            {
                return false;
            }
        }
        return true;
    }
}