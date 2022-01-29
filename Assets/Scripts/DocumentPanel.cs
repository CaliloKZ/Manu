using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentPanel : MonoBehaviour
{
    [SerializeField]
    private ManuelEndScene m_endScene;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            m_endScene.ClosedDocument();
            gameObject.SetActive(false);
        }
    }
}
