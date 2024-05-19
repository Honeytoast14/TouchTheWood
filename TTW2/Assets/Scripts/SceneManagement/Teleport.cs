using System.Collections;
using UnityEngine;

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
    public bool up;
    public bool down;
    public bool leftDown;
    public bool rightDown;
    public bool leftUp;
    public bool rightUp;
    IsometricPlayerMovementController playerController;
    YarnSpinnerFunction yarnSpinnerFunction;
    TitleGame titleGame;

    void Start()
    {
        playerController = FindObjectOfType<IsometricPlayerMovementController>();
        titleGame = FindObjectOfType<TitleGame>();
        yarnSpinnerFunction = FindObjectOfType<YarnSpinnerFunction>();
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
            SetPlayerFaceTo(player.GetComponent<Animator>());

            yield return new WaitForSeconds(0.2f);
            fadeTransition.SetTrigger("Fade Out");
            yield return new WaitForSeconds(0.09f);
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
        SetPlayerFaceTo(player.GetComponent<Animator>());
        // yarnSpinnerFunction.LoadYarnCommand();
        playerController.ResumeMoving();

        yield return new WaitForSeconds(0.15f);
        fadeTransition.SetTrigger("Fade Out");
    }

    private void SetPlayerFaceTo(Animator animator)
    {
        if (left)
        {
            //playerFaceTo.Play("Stable_left");
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 0);
            Debug.Log("Set face to left");
        }
        else if (right)
        {
            // playerFaceTo.Play("Stable_right");
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", 0);
            Debug.Log("Set face to right");
        }
        else if (down)
        {
            // playerFaceTo.Play("Stable_Down");
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", -1);
            Debug.Log("Set face to down");
        }
        else if (up)
        {
            // playerFaceTo.Play("Stable_Up");
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 1);
            Debug.Log("Set face to up");
        }
        else if (leftDown)
        {
            // playerFaceTo.Play("Stable_Down_Left");
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", -1);
            Debug.Log("Set face to leftDown");
        }
        else if (rightDown)
        {
            // playerFaceTo.Play("Stable_Down_Right");
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", -1);
            Debug.Log("Set face to rightDown");
        }
        else if (leftUp)
        {// playerFaceTo.Play("Stable_Up_Left");
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 1);
            Debug.Log("Set face to leftUp");
        }
        else if (rightUp)
        { // playerFaceTo.Play("Stable_Up_Right");
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", 1);
            Debug.Log("Set face to rightUp");
        }
        else
        {
            Debug.LogError("Please set the player face");
        }
    }
}
