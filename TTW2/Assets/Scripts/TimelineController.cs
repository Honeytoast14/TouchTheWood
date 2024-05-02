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
            GameController.Instance.state = GameState.Timeline;
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

    // public void SetSprite(Sprite spriteImage)
    // {
    //     SpriteRenderer spriteRenderer = objectSprite.GetComponent<SpriteRenderer>();
    //     spriteRenderer.sprite = spriteImage;
    // }
}
