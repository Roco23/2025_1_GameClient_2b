
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemData
{
    public int id;
    public string itemName;
    public string description;
    public string nameEng;
    public string itemTypeString;
    [NonSerialized]
    public itemType itemType;
    public int price;
    public int power;
    public int level;
    public bool isStackable;
    public string iconPath;

    public void InitalizeEnums()
    {
        if (Enum.TryParse(itemTypeString, out itemType parsedType))
        {
            itemType = parsedType;
        }
        else
        {
            Debug.LogError($"아이템 |{itemName}에 유요하지 않은 아이템 타임 : {itemTypeString}");
            itemType = itemType.Consumable;
        }
    }

}
