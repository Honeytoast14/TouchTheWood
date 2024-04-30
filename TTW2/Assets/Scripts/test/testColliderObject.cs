using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testColliderObject : MonoBehaviour
{
    Animator playerAnimator;
    IsometricPlayerMovementController playerController;
    TitleGame titleGame;
    [SerializeField] string sceneName;
    void Start()
    {
        playerController = FindObjectOfType<IsometricPlayerMovementController>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        titleGame = FindObjectOfType<TitleGame>();
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            // playerController.StopMoving();

            // playerAnimator.SetFloat("Horizontal", -1);
            // playerAnimator.SetFloat("Vertical", 0);
            // Debug.Log($"Horizontal: {playerAnimator.GetFloat("Horizontal")} \n Vertical: {playerAnimator.GetFloat("Vertical")}");

            // StartCoroutine(TestAnimation(playerAnimator));
            titleGame.LoadScene("TestTimeline 1");
        }
    }

    IEnumerator TestAnimation(Animator playerAnimator)
    {
        yield return new WaitForSeconds(0.5f);
        playerController.ResumeMoving();
    }

    public void TestChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
