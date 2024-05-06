using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstFloor : MonoBehaviour
{
    [SerializeField] NPCData npcData;
    [SerializeField] GameObject colider;
    TriggerEvent triggerEvent;

    void Start()
    {
        triggerEvent = FindObjectOfType<TriggerEvent>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (npcData != null)
                triggerEvent.StartDialogue(npcData.dialogueName);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            colider.SetActive(false);
        }
    }
}
