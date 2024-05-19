using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PuzzleWall : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [Header("Tile Change")]
    [SerializeField] Tile tileChange;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Vector3Int position;

    [Header("Dialogue")]
    [SerializeField] NPCData npcData;

    [Header("Animator")]
    Animator emoji;
    [Header("Sound Wall Hide")]
    [SerializeField] AudioClip soundWall;
    bool interactable = false;
    public bool destroyObject { get; set; } = false;
    TriggerEvent triggerEvent;
    SoundPlayer soundPlayer;
    SpriteRenderer emojiSprite;
    public bool useSwitch { get; set; } = false;

    private bool isSoundPlaying = false;

    void Start()
    {
        triggerEvent = FindObjectOfType<TriggerEvent>();
        GameObject soundPlayerObject = GameObject.FindGameObjectWithTag("Audio");
        if (soundPlayerObject != null)
        {
            soundPlayer = soundPlayerObject.GetComponent<SoundPlayer>();
        }

        GameObject emojis = GameObject.FindGameObjectWithTag("Emoji");

        if (emojis != null)
        {
            emojiSprite = emojis.GetComponent<SpriteRenderer>();
            if (emojiSprite != null)
            {
                emojiSprite.enabled = false;
            }
            emoji = emojis.GetComponent<Animator>();
            if (emoji != null)
            {
                Debug.Log("Find Emoji");
            }
        }
        else
        {
            Debug.LogError("Emoji GameObject not found.");
        }
    }


    void Update()
    {
        if (interactable)
        {
            if (Input.GetKeyDown(KeyCode.Z) && GameController.Instance.state == GameState.FreeRoam)
            {
                if (!useSwitch)
                {
                    if (npcData != null)
                    {
                        triggerEvent.StartDialogue(npcData.dialogueName);
                    }
                }
            }

            if (useSwitch)
                tilemap.SetTile(position, tileChange);

            if (wall != null && destroyObject)
            {
                HideWall(wall);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactable = false;
        }
    }

    void HideWall(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
            if (!isSoundPlaying)
            {
                StartCoroutine(PlayWallSound());
            }
        }
    }

    public void SetEmoji(string animationName, string objectName)
    {
        GameObject puzzleGameObject = GameObject.Find(objectName);
        if (puzzleGameObject == null)
        {
            Debug.LogError($"GameObject with name {objectName} not found.");
            return;
        }

        PuzzleWall puzzleWall = puzzleGameObject.GetComponent<PuzzleWall>();
        if (puzzleWall == null)
        {
            Debug.LogError($"PuzzleWall component not found on GameObject {objectName}.");
            return;
        }

        puzzleWall.StartCoroutine(SetEmojiTime(animationName));

        if (puzzleWall.soundPlayer != null)
        {
            puzzleWall.soundPlayer.PlaySFX(soundPlayer.switchUsed);
        }
        else
        {
            Debug.LogError("soundPlayer is null in the referenced PuzzleWall.");
        }

        puzzleWall.useSwitch = true;

        if (puzzleWall.wall != null)
            puzzleWall.destroyObject = true;
    }

    public void SetEmojiNoSwitch(string animationName, string objectName)
    {
        GameObject puzzleGameObject = GameObject.Find(objectName);
        if (puzzleGameObject == null)
        {
            Debug.LogError($"GameObject with name {objectName} not found.");
            return;
        }

        PuzzleWall puzzleWall = puzzleGameObject.GetComponent<PuzzleWall>();
        if (puzzleWall == null)
        {
            Debug.LogError($"PuzzleWall component not found on GameObject {objectName}.");
            return;
        }

        puzzleWall.StartCoroutine(SetEmojiTime(animationName));
    }

    private IEnumerator SetEmojiTime(string animationName)
    {
        if (emoji != null && emojiSprite != null)
        {
            emojiSprite.enabled = true;
            emoji.Play(animationName);
            yield return new WaitForSeconds(0.8f);
            emojiSprite.enabled = false;
        }
    }

    private IEnumerator PlayWallSound()
    {
        isSoundPlaying = true;
        if (soundWall != null && soundPlayer != null)
        {
            soundPlayer.PlaySFX(soundWall);
            destroyObject = false;
        }
        yield return new WaitForSeconds(soundWall.length);
        isSoundPlaying = false;
    }
}
