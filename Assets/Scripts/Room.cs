using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D m_confiner;
    [SerializeField]
    private List<Transform> m_playerStartPos = new List<Transform>();

    public PolygonCollider2D GetConfiner()
    {
        return m_confiner;
    }

    public Transform GetStartPos(int index)
    {
        return m_playerStartPos[index];
    }


}
