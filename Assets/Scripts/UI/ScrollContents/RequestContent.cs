using UnityEngine;
using UnityEngine.UI;

public class RequestContent : MonoBehaviour
{
    [SerializeField]
    private Image profile;

    [SerializeField]
    private Text nameTxt;
    int id;

    GameObject parent;
    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetImage(UI_Color.Colors color)
    {
        Debug.Log(color.ToString());
    }

    public void SetName(string name)
    {
        nameTxt.text = name;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void AcceptRequest()
    {
        parent.GetComponent<UI_Friend>().AddFriend(nameTxt.text, id);
        DeleteRequest();
    }

    public void DeleteRequest()
    {
        parent.GetComponent<UI_Friend>().DeleteRequest(this.gameObject);
    }
}
