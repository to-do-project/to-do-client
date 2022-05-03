using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{

/*    GameObject root;
    ItemController itemController;*/

    public Action<Collider2D> OnCollisionEvent = null;
    public Action<Collider2D> OnCollisionExitEvent = null;

/*    void Start()
    {
        Init();
    }

    void Init()
    {
        root = transform.parent.parent.parent.gameObject;
        itemController = Util.FindChild<ItemController>(root, "ItemInner", true);
    }*/


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (OnCollisionEvent != null)
        {
            OnCollisionEvent.Invoke(collision);
        }


    }

/*    void OnTriggerExit2D(Collider2D collision)
    {
        if(OnCollisionExitEvent!= null)
        {
            OnCollisionExitEvent.Invoke(collision);
        }
    }*/
}
