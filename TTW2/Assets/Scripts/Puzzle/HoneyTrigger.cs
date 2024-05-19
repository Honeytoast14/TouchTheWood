using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoneyTrigger : MonoBehaviour
{
    bool inZone = false;
    [SerializeField] GameObject parentObject;
    void Update()
    {
        if (inZone && Input.GetKeyDown(KeyCode.Z) && GameController.Instance.state == GameState.FreeRoam)
        {
            if (parentObject.name == "PuzzleHoneyEasy")
            {
                if (!PuzzleHoneyEasy.Instance.win)
                    PuzzleHoneyEasy.Instance.OpenEasyHon();
            }
            if (parentObject.name == "PuzzleHoneyMid")
            {
                if (!PuzzleHoneyMed.Instance.win)
                    PuzzleHoneyMed.Instance.OpenMedHon();
            }
            if (parentObject.name == "PuzzleHoneyHard")
            {
                if (!PuzzleHoneyHard.Instance.win)
                    PuzzleHoneyHard.Instance.OpenHardHon();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inZone = false;
        }
    }
}
