using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleGame : MonoBehaviour
{
    [SerializeField] List<Button> disableButtons;

    public GameObject loadingScene;
    public Slider loadingBar;
    void Start()
    {
        foreach (Button button in disableButtons)
        {
            button.interactable = false;
        }
    }

    IEnumerator LoadSceneAsyn(string sceneName)
    {
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
        Application.Quit();
    }
    public void LoadScene(string nameScene)
    {
        StartCoroutine(LoadSceneAsyn(nameScene));
    }

}
