using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetController : MonoBehaviour
{
    float timer =0f;
    float maxTime = 0.5f;

    void Start()
    {
        Debug.Log("Planet");
    }

    void Update()
    {
        
    }


    private void OnMouseDrag()
    {
        if (Managers.UI.checkPopupOn()) return;

        if (timer >= maxTime)
        {
            timer = 0f;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Managers.Scene.LoadScene(Define.Scene.Edit);
            }
        }
        else
        {
            timer += Time.deltaTime;
        }

    }
}
