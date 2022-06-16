using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FriendMain : UI_Popup
{
    GameObject planet;
    GameObject character;

    public void InitView(ResponseMainPlanet data)
    {
        //�༺, ������, ĳ���� ����
        PlanetInstantiate(data.planetColor, data.level);
        CharacterInstantiate(data.characterItem);
        // ItemInstantiate(data.planetItemList);
    }

    private void PlanetInstantiate(string color, int level)
    {
        //blue,green,red �� �������� Ȯ�� �� �ش� �༺ ����
        //������ Ȯ��

        if (planet == null)
        {
            string path = "Planet/" + color + "_" +
                    level.ToString();

            planet = Managers.Resource.Instantiate(path);
            planet.transform.position = new Vector3(0, 0, 105);
        }

    }


    //ĳ���� ����
    void CharacterInstantiate(long characterColor)
    {
        //Debug.Log("character ");
        if (planet != null)
        {
            if (character == null)
            {
                string path = "Character/" + "Character_" + characterColor.ToString();
                Vector3 pos = new Vector3(0, 5.81f, planet.transform.position.z);

                character = Managers.Resource.Instantiate(pos, path, planet.transform);
            }
        }
    }

    //������ ����
    private void ItemInstantiate(List<MainItemList> list)
    {
        Transform[] childList = planet.transform.GetChild(2).GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != planet.transform.GetChild(2))
                {
                    Managers.Resource.Destroy(childList[i].gameObject);
                }
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            string path = "Items/" + list[i].itemCode;

            for (int j = 0; j < list[i].positionList.Count; j++)
            {
                Vector3 pos = new Vector3(list[i].positionList[j].posX, list[i].positionList[j].posY, planet.transform.GetChild(2).transform.position.z);
                GameObject tmp = Managers.Resource.Instantiate(pos, path, planet.transform.GetChild(2).transform);
            }
        }
    }
}
