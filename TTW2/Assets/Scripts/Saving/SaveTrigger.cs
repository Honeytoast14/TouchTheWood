using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class SaveTrigger : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    public bool canSave = false;
    public bool isLoad = false;

    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            if (transform.tag == "SaveSpot")
            {
                Debug.Log("You can save");
                if (SavingSystem.i != null)
                {
                    SavingSystem.i.Save("saveSlot1");
                    dialogueRunner.SaveStateToPersistentStorage("dialogueSaveTest");
                }
            }

            if (transform.tag == "LoadSpot")
            {
                Debug.Log("You can load");
                isLoad = false;
                if (SavingSystem.i != null)
                {
                    SavingSystem.i.Load("saveSlot1");
                    dialogueRunner.LoadStateFromPersistentStorage("dialogueSaveTest");
                }
            }
        }
    }
}
