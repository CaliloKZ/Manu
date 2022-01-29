using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using MEC;

public class NewItemPanel : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private InventoryManager m_inventory;
    [SerializeField]
    private ManuelEndScene m_endScene;
    private Item m_newItem;

    [SerializeField]
    private TextMeshProUGUI m_newItemText;
    [SerializeField]
    private Image m_newItemImage;

    [SerializeField]
    private string m_foundPhotoDialogue;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            OnClose();
            //GameManager.instance.ChangeCanMove(true);
            //gameObject.SetActive(false);
        }
    }

    public void NewItem(Item item)
    {
        m_newItem = item;
        m_inventory.AddItem(m_newItem);
        m_newItemImage.sprite = item.itemSprite;
        m_newItemText.text = item.displayName;
    }

    void OnClose()
    {
        switch (m_newItem.itemName)
        {
            case "PuzzlePiece":
                if (!m_gameManager.foundFirstPuzzlePieces)
                    m_gameManager.FirstPuzzlePiecesFound();
                else
                    m_gameManager.FoundJigsawItem();
                break;
            case "Photo":
                m_gameManager.GotPhoto();
                break;
            case "Key":
                m_gameManager.ChangeCanMove(true);
                break;
            case "OldKey":
                m_gameManager.GotOldKey();
                break;
            case "DocPapers":
                UIManager.instance.ActivateDocumentPanel();
                break;
            case "ManuelPhoto":
                m_endScene.ClosedPhoto();
                break;
            default:
                Debug.LogWarning("Error NewItemPanel.Onclose, var m_newItem.itemName = " + m_newItem.itemName);
                break;
        }
        gameObject.SetActive(false);
    }
}
