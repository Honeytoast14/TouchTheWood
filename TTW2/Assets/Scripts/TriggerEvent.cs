using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class TriggerEvent : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    [SerializeField] NPCData npcData;
    public NPCData NPCData => npcData;
    Animator animator;
    Rigidbody2D rb;
    GameObject player;

    public bool canTalk = false;
    private bool interactable = false;
    private bool isCurrentConversation = false;

    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponentInParent<Animator>();
        rb = player.gameObject.GetComponent<Rigidbody2D>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        dialogueRunner.onDialogueStart.AddListener(UseCoolDownTalk);
        dialogueRunner.onDialogueComplete.AddListener(EndDialouge);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && interactable && !dialogueRunner.IsDialogueRunning)
        {
            StartDialogue();
        }
    }

    private void UseCoolDownTalk()
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
        if (collision.gameObject.tag == "Player" && transform.parent.tag == "NPC")
        {
            interactable = true;
            Debug.Log($"Enter {transform.parent.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.parent.tag == "NPC")
        {
            interactable = false;
            Debug.Log($"Exit {transform.parent.name}");
        }
    }

    private void EndDialouge()
    {
        if (isCurrentConversation)
        {
            Debug.Log("Dialogue is complete");

            player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            animator.SetBool("IsTalking", true);

            isCurrentConversation = false;

        }
        canTalk = false;
    }

    public void StartDialogue()
    {
        Debug.Log($"Started conversation with {transform.parent.name}.");
        isCurrentConversation = true;

        if (npcData != null)
        {
            dialogueRunner.StartDialogue(npcData.dialogueID);

            player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = false;
            player.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}