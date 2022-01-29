using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_slots = new List<GameObject>();
    [SerializeField]
    private TextMeshProUGUI m_secondSlotText;
    private int m_slotsFilled;

    public void AddItem(Item item)
    {
        Debug.Log("inventoryManager item = " + item);
        Debug.Log("inventoryManager itemName = " + item.itemName);
        if (item.itemName == "PuzzlePiece")
        {
            if (GameManager.instance.foundFirstPuzzlePieces)
            {
                m_secondSlotText.text = (GameManager.instance.jigsawItems + 1 + "x");
                return;
            }
        }
        m_slots[m_slotsFilled].SetActive(true);
        m_slots[m_slotsFilled].GetComponent<Image>().sprite = item.itemSprite;
        m_slotsFilled++;
    }
}
