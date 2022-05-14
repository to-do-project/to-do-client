using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSet : MonoBehaviour
{
    [SerializeField]
    Sprite onImage;

    [SerializeField]
    Sprite offImage;

    public Sprite GetImage(bool toggle)
    {
        return toggle ? onImage : offImage;
    }
}
