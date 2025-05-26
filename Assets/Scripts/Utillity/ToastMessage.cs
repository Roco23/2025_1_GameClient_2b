using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;


public class ToastMessage : MonoBehaviour
{
    public static ToastMessage Instance { get; private set; }

    [SerializeField] private GameObject toastPrefab;
    [SerializeField] private Transform messageContainer;
    [SerializeField] private float displayTime = 2.5f;
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private int maxMessage = 5;

    private Queue<GameObject> messageQueue = new Queue<GameObject>();
    private List<GameObject> activeMessage = new List<GameObject>();
    private bool isProcessingQueue = false;

    public enum MessageType
    {
        Normal,
        Success,
        Warning,
        Error,
        info,
    }

    private IEnumerator ProcessMessageQueue()
    {
        isProcessingQueue = true;

        while(messageQueue.Count > 0)
        {
            GameObject toast = messageQueue.Dequeue();
            if(activeMessage.Count >= maxMessage && activeMessage.Count > 0)
            {
                Destroy(activeMessage[0]);
                activeMessage.RemoveAt(0);
            }
            
            toast.SetActive(true);
            activeMessage.Add(toast);

            CanvasGroup canvasGroup = toast.GetComponent<CanvasGroup>();
            if(canvasGroup == null)
            {
                canvasGroup = toast.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0;
            float elapedTime = 0;
            while (elapedTime < fadeTime)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapedTime / fadeTime);
                elapedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1;

            yield return new WaitForSeconds(displayTime);

            elapedTime = 0;
            while (elapedTime < fadeTime)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapedTime / fadeTime);
                elapedTime += Time.deltaTime;
                yield return null;
            }

            activeMessage.Remove(toast);
            Destroy(toast);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ShowMessage(string message, MessageType type = MessageType.Normal)
    {
        if (toastPrefab == null || messageContainer == null) return;

        GameObject toastInstance = Instantiate(toastPrefab, messageContainer);
        toastInstance.SetActive(false);

        TextMeshProUGUI textComponent = toastInstance.GetComponentInChildren<TextMeshProUGUI>();
        Image backgroundImage = toastInstance.GetComponentInChildren    <Image>();

        if (textComponent != null)
        {
            textComponent.text = message;

            Color textColor;
            Color backgroundColor;


            switch (type)
            {
                case MessageType.Success:
                    textColor = Color.green;
                    backgroundColor = new Color(0.2f, 0.6f, 0.2f, 0.8f);
                    break;
                case MessageType.Warning:
                    textColor = Color.yellow;
                    backgroundColor = new Color(0.8f, 0.6f, 0.2f, 0.8f);
                    break;
                case MessageType.Error:
                    textColor = Color.red;
                    backgroundColor = new Color(0.2f, 0.6f, 0.2f, 0.8f);
                    break;
                case MessageType.info:
                    textColor = Color.blue;
                    backgroundColor = new Color(0.2f, 0.6f, 0.2f, 0.8f);
                    break;
                default:
                    textColor = Color.white;
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                    break;

            }

            textComponent.color = textColor;
            if (backgroundColor != null)
            {
                backgroundImage.color = backgroundColor;
            }

        }

        messageQueue.Enqueue(toastInstance);

        if(!isProcessingQueue)
        {
            StartCoroutine(ProcessMessageQueue());
        }   
    }





    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
