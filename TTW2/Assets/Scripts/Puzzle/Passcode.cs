using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class Passcode : MonoBehaviour
{
    SoundPlayer soundPlayer;
    GameObject parent;
    DialogueRunner dialogueRunner;
    TriggerEvent triggerEvent;
    string Nr = null;
    int NrIndex = 0;
    string codeNumber;
    [SerializeField] GameObject passCodePanel;
    [SerializeField] TMP_Text UiText;
    public static Passcode Instance { get; private set; }
    public bool setYarn = false;
    public bool setUp = false;
    bool isOpen = false;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        triggerEvent = GetComponent<TriggerEvent>();

        if (passCodePanel != null)
            passCodePanel.SetActive(false);

        if (setYarn)
            dialogueRunner.AddCommandHandler<string>("OpenPassCode", OpenPassCode);
    }

    void Update()
    {
        HandleUpdate(null);
        if (isOpen)
        {
            StartCoroutine(OpenPass());
            isOpen = false;
        }
    }

    public void HandleUpdate(Action onBack)
    {
        if (!setUp)
        {
            if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)) && GameController.Instance.state == GameState.Passcode)
            {
                GameController.Instance.state = GameState.FreeRoam;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Nr = null;
                UiText.text = "รหัสผ่าน";

                passCodePanel.SetActive(false);

                onBack?.Invoke();
            }
        }
    }

    public void CodeFunction(string Numbers)
    {
        NrIndex++;
        Nr = Nr + Numbers;
        UiText.text = Nr;
        Debug.Log(Numbers);

        soundPlayer.PlaySFX(soundPlayer.buttonClick);
    }

    public void Enter(string Code)
    {
        if (Nr == Code)
        {
            passCodePanel.SetActive(false);
            triggerEvent.StartDialogue(triggerEvent.npcData.dialogueName + "_correct");

            var variableStorage = FindObjectOfType<InMemoryVariableStorage>();
            bool codeBool;
            variableStorage.TryGetValue("$" + codeNumber, out codeBool);
            variableStorage.SetValue("$" + codeNumber, codeBool = true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            passCodePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            triggerEvent.StartDialogue(triggerEvent.npcData.dialogueName + "_incorrect");

            Nr = null;
            UiText.text = "รหัสผ่าน";
        }
    }

    public void Delete()
    {
        NrIndex++;
        Nr = null;
        UiText.text = "รหัสผ่าน";
    }

    public void OpenPassCode(string name)
    {
        Passcode passcode;
        parent = GameObject.Find(name);
        passcode = parent.GetComponentInChildren<Passcode>();
        passcode.isOpen = true;

        // StartCoroutine(OpenPass(name));
    }

    IEnumerator OpenPass()
    {
        yield return new WaitForSeconds(0.01f);
        GameController.Instance.state = GameState.Passcode;
        passCodePanel.SetActive(true);

        codeNumber = name;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
