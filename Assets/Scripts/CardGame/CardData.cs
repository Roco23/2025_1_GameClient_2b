using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard" , menuName = "Cards/Card Data")]

public class CardData : ScriptableObject
{
    public enum CardType                //카드 타입 열거형 추가
    {
        Attack,     //공격카드
        Heal,       //회복카드
        Buff,       // 버프카드
        Utility     //유틸리티 카드
    }

    public enum AdditionalEffectType            //추가 효과 타입 열거형 추가
    {
        None,               //추가 효과 없음
        DrawCard,           //카드 드로우
        DiscardCard,        //카드 버리기
        GainMana,           //마나 획득
        ReduceEnemyMana,    //적 마나 감소
        ReduceCardCost      //다음 카드 비용감소
    }

    //추가 효과 리스트
    public List<AdditionalEffect> additionalEffects = new List<AdditionalEffect>();

    public string cardName;         //카드 이름
    public string description;      //카드 설명
    public Sprite artwork;          //카드 이미지
    public int manaCost;            //마나 비용
    public int effectAmount;        //공격력/효과 값
    public CardType cardType;       //카드타입

    public Color GetCardColor()
    {
        switch (cardType)
        {
            case CardType.Attack:
                return new Color(0.9f, 0.3f, 0.3f); //발강
            case CardType.Heal:
                return new Color(0.3f, 0.3f, 0.3f);
            case CardType.Buff:
                return new Color(0.3f, 0.3f, 0.9f);
            case CardType.Utility:
                return new Color(0.9f, 0.9f, 0.9f);
            default:
                return Color.white;
        }
    } 


    public string GetAdditionalEffectsDescription()
    {
        if (additionalEffects.Count == 0)
        return "";

        string result = "\n";

        foreach(var effect in additionalEffects)
        {
            result += effect.GetDescription() + "\n";
        }

        return result;
    }
}
