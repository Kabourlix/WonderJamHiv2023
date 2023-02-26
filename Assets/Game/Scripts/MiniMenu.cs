using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_Menu;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(m_Menu);
    }
}
