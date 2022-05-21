using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UI_Color parent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        parent.ColorBtnEnter(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parent.ColorBtnExit(gameObject);
    }

    void Start()
    {
        parent = FindObjectOfType<UI_Color>();
    }
}
