using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class TriggerEvent : MonoBehaviour
{
    [Header("NPCData")]
    [SerializeField] public NPCData npcData;
    [Header("Item Giver")]
    public ItemGiver itemGiver;
    [Header("Pick Up")]
    public PickUp pickUp;
    private DialogueRunner dialogueRunner;
    GameController gameController;
    Rigidbody2D rb;
    GameObject player;

    public bool canTalk = false;
    private bool interactable = false;
    private bool isCurrentConversation = false;

    void Start()
    {
        player = GameObject.Find("Player");
        //itemGiver = GetComponent<ItemGiver>();
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
            StartDialogue();
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

    public void StartDialogue()
    {
        if (npcData != null)
        {
            isCurrentConversation = true;
            dialogueRunner.StartDialogue(npcData.dialogueID);
        }

        gameController.state = GameState.Dialogue;

        player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = false;
        player.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}