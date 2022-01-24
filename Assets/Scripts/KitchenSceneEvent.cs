using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenSceneEvent : MonoBehaviour
{
    [SerializeField]
    private KitchenSceneDialogue m_kitchenDialogue;

    void StartMomDialogue()
    {
        m_kitchenDialogue.StartDialogue();
    }
}
