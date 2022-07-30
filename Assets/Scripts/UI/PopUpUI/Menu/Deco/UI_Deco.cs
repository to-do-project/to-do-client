using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Deco : UI_PopupMenu
{
    // ���ε� �� �ڽ� ������Ʈ �̸���
    enum Buttons
    {
        Back_btn,
        Done_btn,
        Cancel_btn,
        Left_btn,
        Right_btn,
    }

    enum Images
    {
        Cloth_img,
    }
    // ================================ //

    ItemImageContainer itemImages; // ������ �̹��� ���� ��ũ��Ʈ

    GameObject invenScroll, invenContent, invenButton; // Scroll, Content, Button ������Ʈ
    Image clothImage;         // ĳ���� �� �̹���
    Button leftBtn, rightBtn; // ��, �� ���� ��ư

    Dictionary<long, GameObject> contentList; // Content�� �ڽ� ������ ��ư ������Ʈ�� Dictionary

    int index = 0, startIndex = 0; // ���� ������ �ε���
    bool onSave = false; // �� ��� üũ

    // ���
    const string itemPath = "Prefabs/UI/ScrollContents/Item_btn";
    const string fadePath = "UI/Popup/Menu/Deco/";

    // �ʱ�ȭ
    public override void Init()
    {
        base.Init();

        CameraSet();

        contentList = new Dictionary<long, GameObject>();

        SetBtns();

        Bind<Image>(typeof(Images));
        clothImage = GetImage((int)Images.Cloth_img);

        if (invenContent == null)
        {
            invenContent = GameObject.Find("InvenContent");
        }
        if (invenScroll == null)
        {
            invenScroll = GameObject.Find("InvenScroll");
        }

        itemImages = GetComponent<ItemImageContainer>();

        SetInven(); // �κ� �ʱ�ȭ
    }

    void SetBtns()
    {
        Bind<Button>(typeof(Buttons));

        // �ڷΰ��� ��ư
        SetBtn((int)Buttons.Back_btn, ClosePopupUI);

        // �Ϸ� ��ư || �� ��� �Լ� ����
        SetBtn((int)Buttons.Done_btn, (data) => { SaveInven(); });

        // ��� ��ư || �ʱⰪ���� ����
        SetBtn((int)Buttons.Cancel_btn, (data) => {
            index = startIndex; // ���� �������� ���� ������ �ε����� ����
            ChangeCloth(dataContainer.invenIdList[index]); // �ε����� ���� �� ����

            // �˾� �˸� ���� �� ��ư�� ���
            Managers.Resource.Instantiate(fadePath + "CancelFadeView");
            Managers.Sound.PlayPopupSound();
        });

        // ���� ��ư Ŭ�� || �ε����� -1 �Ͽ� ��ư ��ĭ �̵�
        leftBtn = GetButton((int)Buttons.Left_btn);
        BindEvent(leftBtn.gameObject, (data) => {
            // �ε��� -1 �� �� ����, ��ư�� ���
            if (index == 0) return;
            index--; 

            ChangeCloth(dataContainer.invenIdList[index]); 
            Managers.Sound.PlayNormalButtonClickSound();
        }, Define.TouchEvent.Touch);

        // ������ ��ư Ŭ�� || �ε����� +1 �Ͽ� ��ư ��ĭ �̵�
        rightBtn = GetButton((int)Buttons.Right_btn);
        BindEvent(rightBtn.gameObject, (data) => {
            // �ε��� +1 �� �� ����, ��ư�� ���
            if (index == dataContainer.invenIdList.Count - 1) return;
            index++; 

            ChangeCloth(dataContainer.invenIdList[index]);
            Managers.Sound.PlayNormalButtonClickSound();
        }, Define.TouchEvent.Touch);
    }

    void Start()
    {
        Init();
    }

    // �κ� �ʱ�ȭ
    void SetInven()
    {
        // ��ũ�� ������(��ư) �ʱ�ȭ
        InitContents();
        // ��, �� ��ư �ʱ�ȭ
        RefreshBtnLR();

        // �ε��� �ʱ�ȭ
        SetIndex(Managers.Player.GetInt(Define.CHARACTER_ITEM));
        startIndex = index;
        // �� �ʱ�ȭ
        ChangeCloth(dataContainer.invenIdList[index]);
    }

    // ��ũ�� ������(��ư) �ʱ�ȭ
    void InitContents()
    {
        // �����Ϳ� ����� ����Ʈ�� Ž��
        foreach (var tmp in dataContainer.invenIdList)
        {
            // ������Ʈ ����
            GameObject item = Instantiate(Resources.Load<GameObject>(itemPath));

            // ������Ʈ�� invenContent�� �ڽ����� ����
            item.transform.SetParent(invenContent.transform, false);

            // ��ư transition �Ӽ��� none���� ����, ��ư ��� ȸ������ ����
            item.GetComponent<Button>().transition = Selectable.Transition.None;
            item.transform.Find("Base").GetComponent<Image>().color = new Color(217f / 255f, 217f / 255f, 217f / 255f);

            // ��ư �ʱ�ȭ
            UI_ItemBtn btn = item.GetComponent<UI_ItemBtn>();
            btn.SetValue(tmp, invenScroll.GetComponent<ScrollRect>(), itemImages.CharItemSprites[tmp - itemImages.CharGap]);
            btn.SetDecoScript(GetComponent<UI_Deco>());

            // Dictionary�� ������Ʈ �߰�
            contentList.Add(tmp, item);
        }
    }

    // ��, �� ��ư �缳��
    void RefreshBtnLR()
    {
        // �� ��� �� �� ��� ����
        if (onSave) return;

        // index�� 0�̸� �� ��ư ��Ȱ��ȭ
        if (index == 0)
        {
            leftBtn.interactable = false;
        }
        else
        {
            leftBtn.interactable = true;
        }

        // index�� ����Ʈ�� ���̸� �� ��ư ��Ȱ��ȭ
        if (index == dataContainer.invenIdList.Count - 1)
        {
            rightBtn.interactable = false;
        }
        else
        {
            rightBtn.interactable = true;
        }
    }

    // �ε��� ����
    // id = ������ id��
    void SetIndex(long id)
    {
        // ����Ʈ�� id�� ���� �������� index�� ���Ѵ�
        for (int i = 0; i < dataContainer.invenIdList.Count; i++)
        {
            if (dataContainer.invenIdList[i] == id) index = i;
        }
    }

    // ĳ���� �� ����
    // id = �� ������ id��
    public void ChangeCloth(long id)
    {
        // �� ��� �� �� ��� ����
        if (onSave) return;

        // ���� ��ư ũ�� ��� �� ���� ����
        if (invenButton != null)
        {
            invenButton.transform.localScale = new Vector3(1, 1, 1);
            invenButton.transform.Find("Base").GetComponent<Image>().color = new Color(217f / 255f, 217f / 255f, 217f / 255f);
        }
        // ���� ��ư���� ����, Ȯ�� �� ���� ����
        invenButton = contentList[id];
        invenButton.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        invenButton.transform.Find("Base").GetComponent<Image>().color = new Color(1, 1, 1);

        // ĳ������ �� �̹��� ����
        clothImage.sprite = itemImages.CharItemSprites[id - itemImages.CharGap];

        // ������ ScrollView ������ ����
        ContentSizeFitter fitter = invenContent.GetComponent<ContentSizeFitter>();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);

        // ���� ������ �ε��� ���� �� ��, �� ��ư �缳��
        SetIndex(id);
        RefreshBtnLR();
    }

    // ĳ���� �� ���� �� ���
    void SaveInven()
    {
        // �̹� ����� �� ��� ����
        if (onSave) return;
        onSave = true;

        // �� ��� ��� ����
        string[] hN = { Define.JWT_ACCESS_TOKEN,
                        "User-Id" };
        string[] hV = { Managers.Player.GetString(Define.JWT_ACCESS_TOKEN),
                        Managers.Player.GetString(Define.USER_ID) };

        // ĳ���� �� ���� �� ���
        Managers.Web.SendUniRequest("api/closet/character-items/" + dataContainer.invenIdList[index].ToString(), "PATCH", null, (uwr) => {
            // �� ���� json �����͸� ����Ƽ �����ͷ� ��ȯ
            Response<string> response = JsonUtility.FromJson<Response<string>>(uwr.downloadHandler.text);

            // �� ��� ���� ��
            if (response.isSuccess)
            {
                // Debug.Log(response.result);
                // PlayerPrefs�� ����� Character Item ����
                Managers.Player.SetInt(Define.CHARACTER_ITEM, (int)dataContainer.invenIdList[index]);

                // ����ȭ�� ĳ���� ������Ʈ
                Managers.Player.UpdateCharacterItem(dataContainer.invenIdList[index]);

                // �˾� �˸� ���� �� ȿ���� ���
                Managers.Resource.Instantiate(fadePath + "DoneFadeView");
                Managers.Sound.PlayPopupSound();
            }
            // ��ū ��߱� ���� ��
            else if(response.code == 6000)
            {
                // ��߱� �� ������� ���� ���� �ʱ�ȭ
                onSave = false;
                Debug.Log(response.message);
                // ��ū ��߱�
                Managers.Player.SendTokenRequest(SaveInven);
            }
            // �� ��� ���� ��
            else
            {
                Debug.Log(response.message);
            }
            onSave = false;
        }, hN, hV);
    }
}
