using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace DS
{
    public class TriggerEvent : MonoBehaviour
    {
        public DialogueRunner dialogueRunner;
        public UnityEvent onDialogueComplete;
        public NPCData npcData;

        Animator animator;
        IsometricPlayerMovementController isometricPlayerMovementController;
        Rigidbody2D rb;
        GameObject player;

        void Start()
        {
            isometricPlayerMovementController = FindObjectOfType<IsometricPlayerMovementController>();
            player = GameObject.Find("Player");
            animator = GetComponentInParent<Animator>();
            rb = player.gameObject.GetComponent<Rigidbody2D>();
            dialogueRunner.onDialogueComplete.AddListener(() => MakePlayerMove());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                isometricPlayerMovementController.canTalk = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                isometricPlayerMovementController.canTalk = false;
            }
        }

        void MakePlayerMove()
        {
            player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            animator.SetBool("IsTalking", false);
        }

        public void StartDialogue()
        {
            if (npcData != null)
            {
                Debug.Log("Dialogue is Starting!");

                dialogueRunner.StartDialogue(npcData.dialogueID);
                player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = false;
                player.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                animator.SetBool("IsTalking", true);
            }
            else
            {
                Debug.LogError("NPCData not assigned for NPC: " + gameObject.name);
            }
        }
    }
}
