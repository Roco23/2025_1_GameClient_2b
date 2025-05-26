using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleCheatSystem : MonoBehaviour
{
    public static SimpleCheatSystem instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private TMP_InputField commandInput;
    [SerializeField] private TextMeshProUGUI outputText;

    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F1;

    private Dictionary<string, System.Action<string[]>> commands;
    private List<string> outputLines = new List<string>();
    private bool isActive = false;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitiallnzeCommands();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (cheatPanel != null)
        {
            cheatPanel.SetActive(false);
        }
        Log("치트 시스템 준비 완료. F1 키로 열기");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanel();
        }
        if (isActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ExecuteCommand();
        }
        
    }

    private void InitiallnzeCommands()
    {
        commands = new Dictionary<string, System.Action<string[]>>
        {
            {"god" , ToggleGodmode },
            {"kill" , KillPlayer},
            { "clear", ClearConsole},
            { "help" , ShowHelp}
        };
    }

    private void ToggleGodmode(string[] args)
    {
        Log("무적 모드 토글됨");
        ShowText("무적모드 ON/OFF", ToastMessage.MessageType.Success);
        // TODO: 실제 무적 모드 구현

    }
    private void KillPlayer(string[] args)
    {
        Log("플레이어 사망");
        ShowText("플레이어 사망!", ToastMessage.MessageType.Error);
    }
    private void ClearConsole(string[] args)
    {
        outputLines.Clear();
        if (outputText != null)
            outputText.text = "";
        Log("콘솔 정리됨");
    }

    private void ShowHelp(string[] args)
    {
        Log("=== 치트 명령어 ===");
        Log("god - 무적모드");
        Log("kill - 플레이어죽이기");
        Log("clear - 콘솔 정리");
    }

    private void Log(string message , bool isError = false)
    {
        string coloredMessage = isError ? $"<color=red>{message}</color>" : message;
        outputLines.Add(coloredMessage);

        if(outputLines.Count > 10)
            outputLines.RemoveAt(0);

        if(outputText != null )
        {
            outputText.text = string.Join("\n", outputLines);
        }
    }

    private void ShowText(string message, ToastMessage.MessageType type = ToastMessage.MessageType.info)
    {
        if(ToastMessage.Instance != null)
        {
            ToastMessage.Instance.ShowMessage($"[치트] {message}", type);
        }
    }

    private void ExecuteCommand()
    {
        if (commandInput == null || string.IsNullOrEmpty(commandInput.text))
            return;
        string command = commandInput.text.Trim().ToLower();
        string[] parts = command.Split(' ');

        Log($"> {command}");

        if(commands.ContainsKey(parts[0]))
        {
            commands[parts[0]](parts);
        }
        else
        {
            Log($" 알수없는 명령어 : {parts[0]}", true);
        }


        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    private void TogglePanel()
    {
        isActive = !isActive;

        if(cheatPanel != null)
        {
            cheatPanel.SetActive(isActive);

            if(isActive && commandInput != null )
            {
                commandInput.Select();
                commandInput.ActivateInputField();
            }
        }
    }
}
