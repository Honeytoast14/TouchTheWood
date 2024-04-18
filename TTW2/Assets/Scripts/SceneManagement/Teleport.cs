using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Animator fadeTransition;

    [Header("In Scene")]
    [SerializeField] Transform inDestination;

    [Header("Another Scence")]
    [SerializeField] Transform outDestination;

    [Header("Face to")]
    [SerializeField] Animator playerFaceTo;
    public bool left, right, front, back, leftFront, rightFront, leftBack, rightBack = false;
    IsometricPlayerMovementController playerController;

    void Start()
    {
        playerController = FindObjectOfType<IsometricPlayerMovementController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Player is in teleport");
            if (inDestination != null)
            {
                StartCoroutine(TeleportPlayer(collider, inDestination));
            }

            if (outDestination != null)
                Debug.Log($"Move to Scene {outDestination.position}");
        }
    }

    private IEnumerator TeleportPlayer(Collider2D player, Transform placeToGo)
    {
        if (fadeTransition != null)
        {
            playerController.StopMoving();
            fadeTransition.SetTrigger("Fade In");
            yield return new WaitForSeconds(1f);

            player.transform.position = placeToGo.position;
            SetPlayerFaceTo();

            yield return new WaitForSeconds(0.2f);
            fadeTransition.SetTrigger("Fade Out");
            playerController.ResumeMoving();
        }
    }

    private void SetPlayerFaceTo()
    {
        if (left)
        {
            //playerFaceTo.Play("Stable_left");
            playerFaceTo.SetFloat("Horizontal", -1);
            playerFaceTo.SetFloat("Vertical", 0);
        }
        else if (right)
        {
            // playerFaceTo.Play("Stable_right");
            playerFaceTo.SetFloat("Horizontal", 1);
            playerFaceTo.SetFloat("Vertical", 0);
        }
        else if (front)
        {
            // playerFaceTo.Play("Stable_Down");
            playerFaceTo.SetFloat("Horizontal", 0);
            playerFaceTo.SetFloat("Vertical", -1);
        }
        else if (back)
        {
            // playerFaceTo.Play("Stable_Up");
            playerFaceTo.SetFloat("Horizontal", 0);
            playerFaceTo.SetFloat("Vertical", 1);
        }
        else if (leftFront)
        {
            // playerFaceTo.Play("Stable_Down_Left");
            playerFaceTo.SetFloat("Horizontal", -1);
            playerFaceTo.SetFloat("Vertical", -1);
        }
        else if (rightFront)
        {
            // playerFaceTo.Play("Stable_Down_Right");
            playerFaceTo.SetFloat("Horizontal", 1);
            playerFaceTo.SetFloat("Vertical", -1);
        }
        else if (leftBack)
        {// playerFaceTo.Play("Stable_Up_Left");
            playerFaceTo.SetFloat("Horizontal", -1);
            playerFaceTo.SetFloat("Vertical", 1);
        }
        else if (rightBack)
        { // playerFaceTo.Play("Stable_Up_Right");
            playerFaceTo.SetFloat("Horizontal", 1);
            playerFaceTo.SetFloat("Vertical", 1);
        }
        else
        {
            Debug.LogError("Please set the player face");
        }
    }
}
