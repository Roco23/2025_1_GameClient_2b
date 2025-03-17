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

            Debug.Log($"로드된 아이템 수 : {itemList.Count}");

            foreach(var item in itemList)
            {
                Debug.Log($"아이템: {EncodeKorean(item.itemName)}, 설명 : {EncodeKorean(item.description)}");
            }
        { 
        else
        }
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다. : {JsonFileName}");


    }

    private String EncodeKorean(String text)
    {
        if (string.isNullOrEmpty(text)) return "";
        byte[] bytes = Encoing.Default.GetBytes(text);
        return Encoing.UTF8.GetString(bytes);

    }

   
}
