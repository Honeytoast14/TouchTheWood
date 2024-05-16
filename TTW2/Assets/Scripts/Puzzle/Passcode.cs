using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
public class Passcode : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    TriggerEvent triggerEvent;
    string Nr = null;
    int NrIndex = 0;
    string codeNumber;
    [SerializeField] GameObject passCodePanel;
    [SerializeField] TMP_Text UiText;
    public static Passcode Instance { get; private set; }
    public bool setYarn = false;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        triggerEvent = GetComponent<TriggerEvent>();

        passCodePanel.SetActive(false);

        if (setYarn)
            dialogueRunner.AddCommandHandler<string>("OpenPassCode", OpenPassCode);
    }
    public void HandleUpdate(Action onBack)
    {
        if (Input.GetKeyDown(KeyCode.X) && GameController.Instance.state == GameState.Passcode)
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
    public void CodeFunction(string Numbers)
    {
        NrIndex++;
        Nr = Nr + Numbers;
        UiText.text = Nr;
        Debug.Log(Numbers);
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
        StartCoroutine(OpenPass());
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
