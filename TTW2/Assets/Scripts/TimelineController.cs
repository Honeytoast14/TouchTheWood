using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;

public class TimelineController : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    [SerializeField] PlayableDirector timeline;
    // [SerializeField] GameObject objectSprite;
    public bool setYarn = false;

    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (setYarn)
        {
            dialogueRunner.AddCommandHandler<string>("PlayTimeline", StartTimelineByYarn);
            dialogueRunner.AddCommandHandler<string>("StopTimeline", StopTimeline);
            dialogueRunner.AddCommandHandler<string>("ResumeTimeline", ResumeTimeline);
        }
    }

    public void StartTimeline()
    {
        if (timeline != null)
            timeline.Play();
    }

    public void StartTimelineByYarn(string timelineObject)
    {
        GameObject timelineYarn = GameObject.Find(timelineObject);
        if (timelineYarn != null)
        {
            PlayableDirector playableDirector = timelineYarn.GetComponent<PlayableDirector>();
            playableDirector.Play();
        }
    }

    public void SetToFreeRoam()
    {
        GameController.Instance.state = GameState.FreeRoam;
    }

    public void SetToTimeline()
    {
        GameController.Instance.state = GameState.Timeline;
    }
    public void SetToDialogue()
    {
        GameController.Instance.state = GameState.Dialogue;
    }

    public void StopTimeline(string timelineObject)
    {
        GameObject timelineYarn = GameObject.Find(timelineObject);
        if (timelineYarn != null)
        {
            PlayableDirector playableDirector = timelineYarn.GetComponent<PlayableDirector>();
            playableDirector.Pause();
        }
    }

    public void ResumeTimeline(string timelineObject)
    {
        GameObject timelineYarn = GameObject.Find(timelineObject);
        if (timelineYarn != null)
        {
            PlayableDirector playableDirector = timelineYarn.GetComponent<PlayableDirector>();
            playableDirector.Resume();
        }
    }
    // public void SetSprite(Sprite spriteImage)
    // {
    //     SpriteRenderer spriteRenderer = objectSprite.GetComponent<SpriteRenderer>();
    //     spriteRenderer.sprite = spriteImage;
    // }
}
