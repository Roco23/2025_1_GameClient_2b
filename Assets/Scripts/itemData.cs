
using UnityEngine;

[Serializable]
public class itemData
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
	public bool isStackble;
	public string iconPath;


	//문자열을 열거형으로 변환하느 매서드
	public void lnitalizeEnums()
	{
		if(Enum.TryParse(itemTypeString. out itemType parsedType))
		{
			itemType = parsedType;
		}
		else
		{
			Debug.LogError($"아이템'{itemName}'에 유효하지 않은 아이템 타입 : {itemTypeString}");

			itemType = itemType.Consumable;
		}

	}
}
