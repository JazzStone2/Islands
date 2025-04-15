
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    
    [Header("UI")]
    public Image image;
    public Text countText;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector]public Item item;
    [HideInInspector]public int count = 1;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        ResfreshCount();
    }
    public void ResfreshCount()
    {
        countText.text = count.ToString();
    }
    //drag And Drop
    public void OnBeginDrag(PointerEventData eventData){
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
