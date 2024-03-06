using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SaveTrigger : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    public bool canSave = false;
    void Update()
    {
        if (canSave)
        {
            if (Input.GetKeyDown(KeyCode.S) && SavingSystem.i != null)
            {
                SavingSystem.i.Save("saveSlot1");
                dialogueRunner.SaveStateToPersistentStorage("dialogueSaveTest");
            }
        }
        if (Input.GetKeyDown(KeyCode.L) && SavingSystem.i != null)
        {
            SavingSystem.i.Load("saveSlot1");
            dialogueRunner.LoadStateFromPersistentStorage("dialogueSaveTest");
        }
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player" && transform.parent.tag == "SaveSpot")
        {
            Debug.Log("You can save");
            canSave = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player" && transform.parent.tag == "SaveSpot")
        {
            Debug.Log("Ypu cannot save");
            canSave = false;
        }
    }
}
