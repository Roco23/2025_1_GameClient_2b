using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public class itemDataLoader : MonoBehaviour
{
    // Start is called before the first frame update    
    void Start()
    {

        [SerializeField]
        private String JsonFileName = "items";

        private List<itemData> itemList;

    void LoadItemData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(JsonFileName);

        if(jsonFile != null)
        {
            byte[] bytes = Encoing.Default.GetBytes(JsonFile.text);
            string correntText = Encoing.UTF8.GetString(bytes);

            itemList = JsonConvert.Deserialize0bject<List<itemData>>(correntText);

            Debug.Log($"�ε�� ������ �� : {itemList.Count}");

            foreach(var item in itemList)
            {
                Debug.Log($"������: {EncodeKorean(item.itemName)}, ���� : {EncodeKorean(item.description)}");
            }
        { 
        else
        }
        {
            Debug.LogError($"JSON ������ ã�� �� �����ϴ�. : {JsonFileName}");


    }

    private String EncodeKorean(String text)
    {
        if (string.isNullOrEmpty(text)) return "";
        byte[] bytes = Encoing.Default.GetBytes(text);
        return Encoing.UTF8.GetString(bytes);

    }

   
}
