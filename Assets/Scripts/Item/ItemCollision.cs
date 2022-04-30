using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    /*GameObject DisableImg;
    bool mine;*/

    public Action<Collider2D> OnCollisionEvent = null;

    /*    private void Start()
        {
            Init();
        }
        private void Init()
        {
            GameObject go = gameObject.transform.parent.gameObject;
            DisableImg = Util.FindChild(go, "disable_img",true);
            mine = true;
        }*/


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (OnCollisionEvent != null)
        {
            OnCollisionEvent.Invoke(collision);
        }


        /*        Debug.Log("面倒 :" + collision.name);

                if (collision.gameObject.CompareTag("Item") && mine)
                {
                    Debug.Log("Item 面倒 : " + collision.name);

                    if (collision.transform.parent.parent.parent.name.Equals(transform.parent.parent.parent.name))
                    {
                        return;
                    }
                    if(collision.transform.parent.parent.parent.position.y< transform.parent.parent.parent.position.y)
                    {
                        Debug.Log("Change height state " + collision.gameObject.name);
                        collision.transform.parent.gameObject.GetComponent<ItemController>().ChangeHeightState(true);              
                    }
                    else
                    {
                        DisableImg.SetActive(true);
                    }

                }
                else if(!collision.gameObject.CompareTag("Item") && mine)
                {
                    DisableImg.SetActive(false);
                }
                else if (!mine)
                {
                    if (collision.gameObject.CompareTag("Item"))
                    {
                        if (collision.transform.parent.parent.parent.name.Equals(transform.parent.parent.parent.name))
                        {
                            return;
                        }
                        else
                        {
                            Debug.Log("构尔何碟塞 " + collision.transform.parent.parent.parent.name);
                            collision.transform.parent.gameObject.GetComponent<ItemController>().ChangeColor(true);
                            gameObject.transform.parent.parent.gameObject.GetComponent<ItemController>().ChangeHeightState(false);

                        }
                    }

                }*/
    }

/*    public void SetMine(bool mine)
    {
        this.mine = mine;
    }*/

}
