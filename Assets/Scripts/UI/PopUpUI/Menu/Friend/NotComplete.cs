using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ģ���� ���� �Ϸ����� ���� ������ ���ƿ並 ������ �� ������ �˾��� ���� ��ũ��Ʈ
/// UI_Fade�� ��ӹ޾� 3�� ���̵� ��-�ƿ� �ȴ�
/// </summary>
public class NotComplete : UI_Fade
{
    [SerializeField]
    private Text text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
