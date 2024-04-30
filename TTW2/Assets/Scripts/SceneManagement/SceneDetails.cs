using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScene;
    public bool isLoad { get; private set; }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            LoadAdditiveScene(gameObject.name);
            GameController.Instance.CurrrentScene(this);

            foreach (var scene in connectedScene)
            {
                scene.LoadAdditiveScene(scene.name);
            }

            if (GameController.Instance.previousScene != null)
            {
                var previouslyLoadScene = GameController.Instance.previousScene.connectedScene;
                foreach (var scene in previouslyLoadScene)
                {
                    if (!connectedScene.Contains(scene) && scene != this)
                    {
                        scene.UnLoadAdditiveScene(gameObject.name);
                    }
                }
            }
        }
    }

    public void LoadAdditiveScene(string scene)
    {
        if (!isLoad)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            isLoad = true;
        }
    }
    public void UnLoadAdditiveScene(string scene)
    {
        if (isLoad)
        {
            SceneManager.UnloadSceneAsync(scene);
            isLoad = false;
        }
    }
}
