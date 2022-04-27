using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    GameObject planet;
    Camera PlanetCamera;

    public override void Clear()
    {
        //throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Main;

        Managers.Input.TouchAction -= EnterArrayMode;
        Managers.Input.TouchAction += EnterArrayMode;

        //青己 积己
        planet = Managers.Resource.Instantiate("Planet/BluePlanet");

        PlanetCamera = GameObject.Find("PlanetCamera").GetComponent<Camera>();

    }

    void Update()
    {
        
    }

    void EnterArrayMode(Define.TouchEvent evt)
    {
        if (evt != Define.TouchEvent.Press)
        {
            return;
        }

        Debug.Log("touch event");

        Vector3 mousePosition;
#if UNITY_EDITOR
        mousePosition = Input.mousePosition;
#else
        mousePosition = Input.GetTouch(0).position;
#endif

        mousePosition = PlanetCamera.ScreenToWorldPoint(mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("Planet");

        Debug.Log(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward , 100f, layerMask);
        Debug.DrawRay(mousePosition, PlanetCamera.transform.forward * 100, Color.red, 10f);
        if (hit)
        {
            Debug.Log(hit.collider.gameObject.name);
            Managers.Scene.LoadScene(Define.Scene.Edit);

        }


    }
}
