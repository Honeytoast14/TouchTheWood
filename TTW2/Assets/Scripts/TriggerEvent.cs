using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Tests;

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
    private DialogueRunner dialogueRunner;
    GameController gameController;
    Rigidbody2D rb;
    GameObject player;
    Quest activeQuest;

    public bool canTalk = false;
    private bool interactable = false;
    private bool isCurrentConversation = false;

    public static TriggerEvent Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        rb = player.gameObject.GetComponent<Rigidbody2D>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        gameController = FindObjectOfType<GameController>();

        dialogueRunner.onDialogueStart.AddListener(UseCoolDownTalk);
        dialogueRunner.onDialogueComplete.AddListener(EndDialouge);

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
                activeQuest.StartQuest();
                questToStart = null;
            }
            else if (activeQuest != null)
            {
                if (activeQuest.canBeComplete())
                {
                    activeQuest.CompleteQuest(player.transform);
                    activeQuest = null;
                }
                else
                {
                    activeQuest.InProgressQuest();
                }
            }
            else if (questToComplete != null)
            {
                var quest = new Quest(questToComplete);
                quest.CompleteQuest(player.transform);
                questToComplete = null;

                Debug.Log($"{quest.Base.Name} is complete!");
            }
            else
            {
                StartDialogue(npcData.dialogueID);
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
            itemGiver.GiveItem(player.GetComponent<IsometricPlayerMovementController>());
        }

        if (pickUp != null && interactable && pickUp.CanBeGiven())
        {
            pickUp.GiveItem(player.GetComponent<IsometricPlayerMovementController>());
        }
    }


    public void UseCoolDownTalk()
    {
        StartCoroutine(CoolDownTalk());
    }

    private IEnumerator CoolDownTalk()
    {
        yield return new WaitForSeconds(0.1f);
        canTalk = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && (transform.parent.tag == "NPC" || transform.parent.tag == "Item"))
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && (transform.parent.tag == "NPC" || transform.parent.tag == "Item"))
        {
            interactable = false;
        }
    }

    private void EndDialouge()
    {
        if (isCurrentConversation)
        {
            player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            isCurrentConversation = false;
        }
        gameController.state = GameState.FreeRoam;

        canTalk = false;
    }

    public void StartDialogue(string npcDialogue)
    {
        if (npcData != null)
        {
            isCurrentConversation = true;
            dialogueRunner.StartDialogue(npcDialogue);
        }

        gameController.state = GameState.Dialogue;

        player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = false;
        player.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}

public class NPCQuestSaveData
{
    public QuestSaveData activeQuest;
}