using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] Animator fadeTransition;

    [Header("In Scene")]
    [SerializeField] Transform inDestination;

    [Header("Another Scence")]
    [SerializeField] string outDestination;
    [SerializeField] Transform positionToGo;

    [Header("Face to")]
    public bool left;
    public bool right;
    public bool front;
    public bool back;
    public bool leftDown;
    public bool rightDown;
    public bool leftUp;
    public bool rightUp;
    Animator playerFaceTo;
    IsometricPlayerMovementController playerController;
    TitleGame titleGame;

    void Start()
    {
        playerController = FindObjectOfType<IsometricPlayerMovementController>();
        titleGame = FindObjectOfType<TitleGame>();
        playerFaceTo = playerController.gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Player is in teleport");
            if (inDestination != null)
            {
                StartCoroutine(TeleportPlayerInScene(collider, inDestination));
            }

            if (outDestination != null && positionToGo != null)
            {
                StartCoroutine(TeleportPlayerToAnotherScene(collider, outDestination, positionToGo));
            }
        }
    }

    private IEnumerator TeleportPlayerInScene(Collider2D player, Transform placeToGo)
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

    private IEnumerator TeleportPlayerToAnotherScene(Collider2D player, string nameScene, Transform placeToGo)
    {
        playerController.StopMoving();
        fadeTransition.SetTrigger("Fade In");
        yield return new WaitForSeconds(0.9f);

        titleGame.LoadScene(nameScene);
        player.transform.position = placeToGo.position;
        playerController.ResumeMoving();
        SetPlayerFaceTo();

        yield return new WaitForSeconds(0.15f);
        fadeTransition.SetTrigger("Fade Out");
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
        else if (leftDown)
        {
            // playerFaceTo.Play("Stable_Down_Left");
            playerFaceTo.SetFloat("Horizontal", -1);
            playerFaceTo.SetFloat("Vertical", -1);
        }
        else if (rightDown)
        {
            // playerFaceTo.Play("Stable_Down_Right");
            playerFaceTo.SetFloat("Horizontal", 1);
            playerFaceTo.SetFloat("Vertical", -1);
        }
        else if (leftUp)
        {// playerFaceTo.Play("Stable_Up_Left");
            playerFaceTo.SetFloat("Horizontal", -1);
            playerFaceTo.SetFloat("Vertical", 1);
        }
        else if (rightUp)
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
