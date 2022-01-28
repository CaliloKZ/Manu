using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterKeyGame : MonoBehaviour
{
    [SerializeField]
    private KeyGameManager m_keyGameManager;
    [SerializeField]
    private GameObject m_pressEObj;
    private bool m_isIn;


    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            m_keyGameManager.StartDialogue();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = true;
            m_pressEObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = false;
            m_pressEObj.SetActive(false);
        }
    }
}
