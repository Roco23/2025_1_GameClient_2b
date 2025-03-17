
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


	//���ڿ��� ���������� ��ȯ�ϴ� �ż���
	public void lnitalizeEnums()
	{
		if(Enum.TryParse(itemTypeString. out itemType parsedType))
		{
			itemType = parsedType;
		}
		else
		{
			Debug.LogError($"������'{itemName}'�� ��ȿ���� ���� ������ Ÿ�� : {itemTypeString}");

			itemType = itemType.Consumable;
		}

	}
}
