using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //======ITEM DATA=====//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField] int maxNumberOfItems;
    
    //====ITEM SLOT=====//
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    //====ITEM DESCRIPTION SLOT====//
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }


    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //Check to see if slot is already full
        if(isFull)
        {
            return quantity;
        }

        //update name
        this.itemName = itemName;

        //update image
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;

        //update description
        this.itemDescription = itemDescription;

        //update quantity
        this.quantity += quantity;
        if(this.quantity >= maxNumberOfItems)
        {
            quantityText.text = quantity.ToString();
            quantityText.enabled = true;
            isFull = true;
            //Return the leftovers
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        //Update QUANTITY TEXT
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        /*
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
       */
    }

    public void OnLeftClick()
    {
        if(thisItemSelected)
        {
            inventoryManager.UseItem(itemName);
            this.quantity -= 1;
            quantityText.text = this.quantity.ToString();
            if(this.quantity <= 0)
            {
                EmptySlot();
            }
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;
            if(itemDescriptionImage.sprite == null)
            {
                itemDescriptionImage.sprite = emptySprite;
            }
        }
    }

    private void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        itemDescription = "";
        itemName = "";
        this.isFull = false;

        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = emptySprite;
        itemSprite = emptySprite;
    }

/*
    public void OnRightClick()
    {
        if(quantity > 0)
        {
            // Create/Spawn a new item beside the player when player drops an item.
            GameObject itemToDrop = new GameObject(itemName);
            Item newItem = itemToDrop.AddComponent<Item>();
            newItem.quantity = 1;
            newItem.itemName = itemName;
            newItem.sprite = itemSprite;
            newItem.itemDescription = itemDescription;

            // Create and modify the SpriteRenderer
            SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
            sr.sprite = itemSprite;
            sr.sortingOrder = 5;
            sr.sortingLayerName = "Background";

            // Add a Rigidbody2D
            Rigidbody2D rb = itemToDrop.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; // Not affected by gravity.

            // Add a Collider2D
            Collider2D col = itemToDrop.AddComponent<BoxCollider2D>();

            // Set the location
            itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(1.5f, 0, 0);
            itemToDrop.transform.localScale = new Vector3(2f, 2f, 1f);

            // Subtract the item
            this.quantity -= 1;
            quantityText.text = this.quantity.ToString();
            if(this.quantity <= 0)
            {
                EmptySlot();
            }
        }
    }
    */
    
}
