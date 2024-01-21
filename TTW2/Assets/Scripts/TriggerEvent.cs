using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class TriggerEvent : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    public UnityEvent onDialogueComplete;
    public NPCData npcData;

    Animator animator;
    IsometricPlayerMovementController isometricPlayerMovementController;
    Rigidbody2D rb;
    GameObject player;

    private bool isCurrentConversation = false;
    private bool interactable = false;
    void Start()
    {
        isometricPlayerMovementController = FindObjectOfType<IsometricPlayerMovementController>();

        player = GameObject.Find("Player");
        animator = GetComponentInParent<Animator>();
        rb = player.gameObject.GetComponent<Rigidbody2D>();

        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndDialouge);

        StartCoroutine(InputCheckCoroutine());
        Debug.Log($"test {transform.parent.gameObject.name}");
    }

    private IEnumerator InputCheckCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (interactable && !dialogueRunner.IsDialogueRunning)
                {
                    StartDialogue();
                }
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactable = true;
            Debug.Log($"Enter {transform.parent.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
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

            animator.SetBool("IsTalking", false);

            isCurrentConversation = false;
        }
    }

    public void StartDialogue()
    {
        Debug.Log($"Started conversation with {transform.parent.name}. Starting node: {npcData.name}");
        isCurrentConversation = true;

        if (npcData != null)
        {
            dialogueRunner.StartDialogue(npcData.dialogueID);

            player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = false;
            player.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            animator.SetBool("IsTalking", true);
        }
    }
}