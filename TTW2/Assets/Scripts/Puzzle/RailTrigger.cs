using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RailTrigger : MonoBehaviour
{
    [SerializeField] GameObject button;
    bool inZone = false;
    void Update()
    {
        if (inZone && Input.GetKeyDown(KeyCode.Z) && GameController.Instance.state == GameState.FreeRoam)
        {
            PuzzleRail.Instance.OpenPicturePuzzle();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inZone = true;
            button.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inZone = false;
            button.SetActive(false);
        }
    }
}
