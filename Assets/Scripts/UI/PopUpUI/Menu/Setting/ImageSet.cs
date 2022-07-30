using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 토글 버튼에 사용되는 스크립트
public class ImageSet : MonoBehaviour
{
    // on 이미지
    [SerializeField]
    Sprite onImage;

    // off 이미지
    [SerializeField]
    Sprite offImage;

    // 토글
    public Sprite GetImage(bool toggle)
    {
        return toggle ? onImage : offImage;
    }
}
