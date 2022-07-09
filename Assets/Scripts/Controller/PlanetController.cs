using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlanetController : MonoBehaviour
{
    float timer =0f;
    float maxTime = 0.5f;

    void Start()
    {
        //Debug.Log("Planet");
    }

    void Update()
    {
        
    }


    private void OnMouseDrag()
    {

        if (Managers.UI.checkPopupOn()) return;

        if (string.Equals("Edit", Managers.Scene.CurrentSceneName()))
        {
            //Debug.Log("Edit scene ³»ºÎÀÓ");
            return;
        }
        
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
