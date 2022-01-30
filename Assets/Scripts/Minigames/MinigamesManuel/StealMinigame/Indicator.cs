using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    //[SerializeField]
    private Marker m_marker;
    private void Start()
    {
        m_marker = GetComponentInChildren<Marker>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_marker != null)
        {
            m_marker.Stop();
            if (m_marker.isInRightArea)
            {
                GetComponentInParent<StealMinigameController>().Sucess();
            }
            else
            {
                GetComponentInParent<StealMinigameController>().Failed();
            }
        }
    }
}
