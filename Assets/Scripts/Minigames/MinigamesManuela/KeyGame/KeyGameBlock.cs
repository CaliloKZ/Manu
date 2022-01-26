using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGameBlock : MonoBehaviour
{
    [SerializeField]
    private bool m_isLeft;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KeyGamePlayer"))
        {
            var _controller = other.GetComponent<KeyGamePlayerController>();
            //_controller.Hide(m_isLeft);
        }   
    }
}
