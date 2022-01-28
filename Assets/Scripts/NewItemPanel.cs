using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemPanel : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            GameManager.instance.ChangeCanMove(true);
            gameObject.SetActive(false);
        }
    }
}
