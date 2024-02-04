using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Yarn;
using Yarn.Unity;

public class FirstCutSceneManager : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    private PlayableDirector director;

    public Animator animator;
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        dialogueRunner.onDialogueComplete.AddListener(playDirector);

        dialogueRunner.AddCommandHandler("playDirector", playDirector);
        dialogueRunner.AddCommandHandler<float, float>("setCatAnimation", setCatAnimation);
    }
    public void playDirector()
    {
        //Debug.Log("playDirector and dialogue is complete");
        director.playableGraph.GetRootPlayable(0).Play();
    }

    public void pauseScene()
    {
        director.playableGraph.GetRootPlayable(0).Pause();
    }

    public void setCatAnimation(float x, float y)
    {
        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);
    }
}
