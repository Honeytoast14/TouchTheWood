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
        public Rigidbody2D rb;
        public UnityEvent onDialogueComplete;

        IsometricPlayerMovementController isometricPlayerMovementController;
        GameObject player;

        void Start()
        {
            isometricPlayerMovementController = FindObjectOfType<IsometricPlayerMovementController>();
            player = GameObject.Find("Player");
        }

        void Update()
        {
            dialogueRunner.onDialogueComplete.AddListener(() => MakePlayerMove());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Player")
            {
                Debug.Log("Enter in NPC trigger");
                isometricPlayerMovementController.canTalk = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject.name == "Player")
            {
                Debug.Log("Exit the NPC trigger");
                isometricPlayerMovementController.canTalk = false;
            }
        }

        void MakePlayerMove()
        {
            Debug.Log("OnDialogueComplete is work!");

            player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public void StartDialogue()
        {
            Debug.Log("Dialogue is Starting!");

            dialogueRunner.StartDialogue(transform.parent.gameObject.name);
            player.gameObject.GetComponent<IsometricPlayerMovementController>().enabled = false;
            player.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
