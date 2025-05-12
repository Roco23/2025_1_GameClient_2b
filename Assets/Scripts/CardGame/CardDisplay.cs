using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    public CardData cardData;               //ī�� ������
    public int cardIndex;                   //���п����� �ε���(���߿� ���)

    //3D ī�� ���
    public MeshRenderer cardRenderer;               //ī�� ������ (icon or �Ϸ���Ʈ)
    public TextMeshPro nameText;                    //�̸� �ؽ�Ʈ
    public TextMeshPro cosText;                     //��� �ؽ�Ʈ
    public TextMeshPro attackText;                  //���ݷ�/ȿ�� �ؽ�Ʈ 
    public TextMeshPro descriptionText;             //�����ؽ�Ʈ 

    //ī�� ����
    public bool isDragging = false;
    private Vector3 originalPosition;       //�巡�� �� ���� ��ġ

    //���̾� ����ũ
    public LayerMask enemyLayer;            //�� ���̾�
    public LayerMask playerLayer;           //�÷��̾� ���̾�


    private CardManager cardManager;                //ī�� �Ŵ��� ���� �߰�



    // Start is called before the first frame update
    void Start()
    {
        //���̾� ����ũ ����
        playerLayer = LayerMask.GetMask("Player");
        enemyLayer = LayerMask.GetMask("Enemy");

        cardManager = FindObjectOfType<CardManager>();

        SetupCard(cardData);
    }

    //ī�� ������ ����
    public void SetupCard(CardData data)
    {

        cardData = data;


        //3D �ؽ�Ʈ ������Ʈ
        if (nameText != null) nameText.text = data.cardName;
        if (cosText != null) cosText.text = data.manaCost.ToString();
        if (attackText != null) attackText.text = data.effectAmount.ToString();
        if (descriptionText != null) descriptionText.text = data.description;


        //ī�� �ؽ��� ����
        if(cardRenderer != null && data.artwork != null)
        {
            Material cardMaterial = cardRenderer.material;
            cardMaterial.mainTexture = data.artwork.texture;

        }

    }

    private void OnMouseDown()
    {
        //�巹�� ���� �� ���� ��ġ ����
        originalPosition = transform.position;
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if(isDragging)
        {
            //���콺 ��ġ�� ī�� �̵�
            
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);

        }
    }

    private void OnmouseUp()
    {
        isDragging = false;

        //���� ī�� ���� ��ó ��� �ߴ��� �˻� (���� üũ��)
        if(cardData != null)
        {
            float distToDiscard = Vector3.Distance(transform.position, cardManager.discardPosition.position);

            if (distToDiscard < 2.0f)
            {
                cardManager.DiscardCard(cardIndex);
                return;
            }
        }    

        //���⼭ ���� ī�� ��� ����(���� üũ)
        CharacterStats playerStats = null;
        GameObject player0bj = GameObject.FindGameObjectWithTag("player");
        if (player0bj != null)
        {
            playerStats = player0bj.GetComponent<CharacterStats>();
        }

        if (playerStats == null || playerStats.currentMana < cardData.manaCost)
        {
            Debug.Log($"������ �����մϴ�! (�ʿ� {cardData.manaCost} , ���� : {playerStats?.currentMana ?? 0}");
            transform.position = originalPosition;
            return;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool cardUsed = false;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity , enemyLayer))
        {
            CharacterStats enemyStats = hit.collider.GetComponent<CharacterStats>();

            if (enemyStats != null)
            {
                if(cardData.cardType == CardData.CardType.Attack)
                {
                    enemyStats.TakeDamage(cardData.effectAmount);
                    Debug.Log($" {cardData.cardName} ī��� ������ {cardData.effectAmount} �������� �������ϴ�");
                    cardUsed = true;
                }
            }
            else
            {
                Debug.Log("�� ī��� ������ ����Ҽ� �����ϴ�");
            }

        }

        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
        {
            if (playerStats != null)
            {
                if(cardData.cardType == CardData.CardType.Heal)
                {
                    playerStats.Heal(cardData.effectAmount);
                    Debug.Log($" {cardData.effectAmount} ī��� �÷��̾� ü���� {cardData.effectAmount} ȸ�� �߽��ϴ�.");
                    cardUsed = true;
                }
            }
            else
            {
                Debug.Log("�� ī�带 ������ ��� �� �� �����ϴ�");
            }
        }
        if(!cardUsed)
        {
            transform.position = originalPosition;
            if (cardManager != null)
                cardManager.ArrangeHand();
            return;
        }

        //ī�� ��� �� ���� �Ҹ�
        playerStats.UseMana(cardData.manaCost);
        Debug.Log($"������ {cardData.effectAmount} ��� �߽��ϴ� (���� ���� : {playerStats.currentMana})");

        //�߰� ȿ���� �ִ� ��� ó��
        if(cardData.additionalEffects != null && cardData.additionalEffects.Count > 0)
        {
            ProcessAdditionalEffectsAndDiscard();               //�߰� ȿ�� ����
        }
        else
        {
            if(cardManager != null)
                cardManager.DiscardCard(cardIndex);             //�߰� ȿ�� ������ �ٷ� ������
        }



    }

    private void OnMouseUp()
    {

        CharacterStats playerStats = FindObjectOfType<CharacterStats>();
        if (playerStats == null || playerStats.currentMana < cardData.manaCost)
        {
            Debug.Log($"������ �����մϴ�! (�ʿ� : {cardData.manaCost}, ���� : {playerStats?.currentMana ?? 0})");
            transform.position = originalPosition;
            return;
        }




        isDragging = false;

        //����ĳ��Ʈ�� Ÿ�� ����

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //ī�� ��� ���� ���� ����
        bool cardUsed = false;

        //�� ���� ��� �ߴ��� ����
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
        {
            CharacterStats enemyStats = hit.collider.GetComponent<CharacterStats>();

            if(enemyStats != null)
            {
                if(cardData.cardType == CardData.CardType.Attack) //ī�� ȿ���� ���� �ٸ���
                {
                    //���� ī��� ������ �ֱ�
                    enemyStats.TakeDamage(cardData.effectAmount);
                    Debug.Log($"{cardData.cardName} ī��� ������ {cardData.effectAmount} �������� �������ϴ� ");
                    cardUsed = true;
                }
                else
                {
                    Debug.Log("�� ī�带 ������ ����� �� �����ϴ�");
                }
            }
        }
        else if(Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
        {
            //�÷��̾� ���� ���� ����
            //CaracterStats playerStats = hit.collider.GetComponent<CaracterStats>();

            if(playerStats != null)
            {
                if(cardData.cardType == CardData.CardType.Heal)
                {
                    playerStats.Heal(cardData.effectAmount);
                    Debug.Log($"{cardData.cardName} ī��� �÷��̾��� ü���� {cardData.effectAmount} ȸ���߽��ϴ� ");
                    cardUsed = true;
                }
                else
                {
                    Debug.Log("�� ī��� �÷��̾�� ����� �� �����ϴ�");
                }
            }

        }
        else if(cardManager != null)
        {
            float distToDiscard = Vector3.Distance(transform.position, cardManager.discardPosition.position);
            if(distToDiscard < 2.0f)
            {
                cardManager.DiscardCard(cardIndex);
                return;
            }
        }
        //���� ī�� ���� ��ó�� ����ߴ��� �˻�


        //ī�带 ������� �ʾҴٸ� ���� ��ġ�� �ǵ�����
        if (!cardUsed)
        {
            transform.position = originalPosition;
            //���� ������ (�ʿ��� ���)
            cardManager.ArrangeHand();
        }
        else
        {
            //ī�带 ����ߴٸ� ���� ī�� ���̷� �̵�
            if (cardManager != null)
                cardManager.DiscardCard(cardIndex);

            //ī�� ��� �� ���� �Ҹ�(ī�尡 ���������� ���� �� �߰�)

            playerStats.UseMana(cardData.manaCost);
            Debug.Log($"������ {cardData.manaCost} ��� �߽��ϴ�. (���� ���� : {playerStats.currentMana})");
        }

    }

    private void ProcessAdditionalEffectsAndDiscard()
    {
        CardData cardDataCopy = cardData;
        int cardIndexCopy = cardIndex;

        foreach (var effect in cardDataCopy.additionalEffects)
        {
            switch (effect.effectType)
            {
                case CardData.AdditionalEffectType.DrawCard:
                    for (int i = 0; i < effect.effectAmount; i++)
                    {
                        if (cardManager != null)
                        {
                            cardManager.DrawCard();
                        }
                    }
                    Debug.Log($"{effect.effectAmount} ���� ī�带 ��ο� �߽��ϴ�");
                    break;

                case CardData.AdditionalEffectType.DiscardCard:
                    for (int i = 0; i < effect.effectAmount; i++)
                    {
                        if (cardManager != null && cardManager.handCards.Count > 0)
                        {
                            int randomIndex = Random.Range(0, cardManager.handCards.Count);

                            Debug.Log($"���� ī�� ������ : ���õ� �ε��� {randomIndex}, ���� ���� ũ�� : {cardManager.handCards.Count}");

                            if (cardIndexCopy < cardManager.handCards.Count)
                            {
                                if (randomIndex < cardIndexCopy)
                                {
                                    cardManager.DiscardCard(randomIndex);

                                    //���� ���� ī���� �ε����� ���� ī���� �ε������� �۴ٸ� ���� ī���� �ε����� 1 ���� ���Ѿ���
                                    if (randomIndex < cardIndexCopy)
                                    {
                                        cardIndex--;
                                    }
                                }

                                else if (cardManager.handCards.Count > 1)
                                {
                                    //�ٸ� ī�� ����
                                    int newIndex = (randomIndex + 1) % cardManager.handCards.Count;
                                    cardManager.DiscardCard(newIndex);

                                    if (randomIndex < cardIndexCopy)
                                    {
                                        cardIndexCopy--;
                                    }
                                }
                            }
                            else
                            {
                                //cardIndexCopy �� ���̻� ��ȿ���� ���� ��� . �ƹ� ī�峪 ����
                                cardManager.DiscardCard(randomIndex);
                            }
                        }
                        
                    }
                    Debug.Log($"�������� {effect.effectAmount} ���� ī�带 ���Ƚ��ϴ�");
                    break;

                case CardData.AdditionalEffectType.GainMana:
                    GameObject player0bj = GameObject.FindGameObjectWithTag("Player");
                    if (player0bj != null)
                    {
                        CharacterStats playerStats = player0bj.GetComponent<CharacterStats>();
                        if (playerStats != null)
                        {
                            playerStats.GainMana(effect.effectAmount);
                            Debug.Log($"������ {effect.effectAmount} ȹ�� �߽��ϴ� (���� ���� : {playerStats.currentMana})");
                        }
                    }
                    break;

                case CardData.AdditionalEffectType.ReduceCardCost:
                    for (int i = 0; i < cardManager.card0bjects.Count; i++)
                    {
                        CardDisplay display = cardManager.card0bjects[i].GetComponent<CardDisplay>();
                        if(display != null && display != this)
                        {
                            TextMeshPro costText = display.cosText;
                            if(costText != null)
                            {
                                int originalCost = display.cardData.manaCost;
                                int newCost = Mathf.Max(0, originalCost - effect.effectAmount);
                                costText.text = newCost.ToString();
                                costText.color = Color.green;
                            }
                        }
                    }
                    break;
                   

                case CardData.AdditionalEffectType.ReduceEnemyMana:
                            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Ebentg");
                            foreach (var enemy in enemies)
                            {
                                CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
                                if (enemyStats != null)
                                {
                                    enemyStats.UseMana(effect.effectAmount);
                                    Debug.Log($"������ {enemyStats.characterName} �� ������  {effect.effectAmount} ���Ҹ� ���׽��ϴ� ");
                                }
                            }

                            break;

                        }


        }
    }

}
