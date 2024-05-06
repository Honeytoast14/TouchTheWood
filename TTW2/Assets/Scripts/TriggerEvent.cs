using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;

public class TriggerEvent : MonoBehaviour
{
    [Header("NPCData")]
    [SerializeField] public NPCData npcData;

    [Header("Item Giver")]
    public ItemGiver itemGiver;

    [Header("Pick Up")]
    public PickUp pickUp;

    [Header("Quest")]
    [SerializeField] QuestData questToStart;
    [SerializeField] QuestData questToComplete;
    [SerializeField] GameObject questAttention;

    [Header("Add Quest In Yarn")]
    [SerializeField] QuestData questAdd;
    private DialogueRunner dialogueRunner;
    GameController gameController;
    Quest activeQuest;
    IsometricPlayerMovementController playerController;

    public bool canTalk = false;
    private bool interactable = false;
    private bool isCurrentConversation = false;
    public bool setYarn = false;

    public static TriggerEvent Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<IsometricPlayerMovementController>();

        dialogueRunner.onDialogueComplete.AddListener(EndDialouge);

        if (setYarn)
        {
            dialogueRunner.AddCommandHandler("AddQuest", AddQuestInYarn);
        }

        if (itemGiver != null)
        {
            itemGiver.enabled = false;
        }

        if (pickUp != null)
        {
            pickUp.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && interactable && gameController.state == GameState.FreeRoam && !dialogueRunner.IsDialogueRunning)
        {
            if (questToStart != null)
            {
                activeQuest = new Quest(questToStart);
                activeQuest.StartQuest(npcData.dialogueName + "_StartQuest");
                questToStart = null;
            }
            else if (activeQuest != null)
            {
                if (activeQuest.canBeComplete())
                {
                    activeQuest.CompleteQuest(playerController.transform, npcData.dialogueName + "_AfterQuest");
                    questAttention.SetActive(false);
                    activeQuest = null;
                }
                else
                {
                    activeQuest.InProgressQuest(npcData.dialogueName + "_ProgressQuest");
                }
            }
            else if (questToComplete != null)
            {
                var quest = new Quest(questToComplete);
                quest.CompleteQuest(playerController.transform, npcData.dialogueName + "_AfterQuest");
                questToComplete = null;

                Debug.Log($"{quest.Base.Name} is complete!");
            }
            else
            {
                //Debug.Log("Z is pressed from triggerEvent");
                StartDialogue(npcData.dialogueName);
            }

            if (itemGiver != null)
            {
                itemGiver.enabled = true;
            }

            if (pickUp != null)
            {
                pickUp.enabled = true;
            }
        }

        if (itemGiver != null && interactable && itemGiver.CanBeGiven())
        {
            itemGiver.GiveItem(playerController);
        }

        if (pickUp != null && interactable && pickUp.CanBeGiven())
        {
            pickUp.GiveItem(playerController);
        }

        if (questToStart != null)
        {
            if (questAttention != null)
            {
                questAttention.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent.tag == "NPC" || transform.parent.tag == "Item")
        {
            if (collision.gameObject.tag == "Player")
                interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.parent.tag == "NPC" || transform.parent.tag == "Item")
        {
            if (collision.gameObject.tag == "Player")
                interactable = false;
        }
    }

    public void EndDialouge()
    {
        gameController.state = GameState.FreeRoam;

        if (isCurrentConversation)
        {
            playerController.ResumeMoving();
            isCurrentConversation = false;
        }

        canTalk = false;
    }

    public void StartDialogue(string npcDialogue)
    {
        playerController.StopMoving();

        isCurrentConversation = true;
        dialogueRunner.StartDialogue(npcDialogue);

        gameController.state = GameState.Dialogue;
    }

    public void AddQuestInYarn()
    {
        if (questAdd != null)
            questToStart = questAdd;
    }
}

public class NPCQuestSaveData
{
    public QuestSaveData activeQuest;
}