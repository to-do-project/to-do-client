using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ��ư�� ���Ǵ� ��ũ��Ʈ
public class ImageSet : MonoBehaviour
{
    // on �̹���
    [SerializeField]
    Sprite onImage;

    // off �̹���
    [SerializeField]
    Sprite offImage;

    // ���
    public Sprite GetImage(bool toggle)
    {
        return toggle ? onImage : offImage;
    }
}
