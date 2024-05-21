using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleGame : MonoBehaviour
{
    [SerializeField] Animator fadeTransition;
    public GameObject loadingScene;
    public Slider loadingBar;
    GameObject title;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("left click");
        }
    }
    IEnumerator LoadSceneAsyn(string sceneName)
    {
        if (fadeTransition != null)
        {
            fadeTransition.gameObject.SetActive(true);
            fadeTransition.SetTrigger("Fade In");
            yield return new WaitForSeconds(0.9f);
            fadeTransition.gameObject.SetActive(false);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScene.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
    }

    public void ExitGame()
    {
        StartCoroutine(SetFadeIn());

        Application.Quit();
    }
    public void LoadScene(string nameScene)
    {
        StartCoroutine(LoadSceneAsyn(nameScene));
    }

    IEnumerator SetFadeIn()
    {
        fadeTransition.SetTrigger("Fade In");
        yield return new WaitForSeconds(1f);
    }

    public void CloseTitle(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void OpenTitle()
    {
        title.SetActive(true);
    }
}
