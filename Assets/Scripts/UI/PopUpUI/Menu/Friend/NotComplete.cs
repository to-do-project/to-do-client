using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 친구가 아직 완료하지 않은 투두의 좋아요를 눌렀을 때 나오는 팝업에 들어가는 스크립트
/// UI_Fade를 상속받아 3초 페이드 인-아웃 된다
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
