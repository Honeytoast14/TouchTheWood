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
        Debug.Log("PuzzleWall Start");
        triggerEvent = FindObjectOfType<TriggerEvent>();
        soundPlayer = FindObjectOfType<SoundPlayer>();

        GameObject emojis = GameObject.FindGameObjectWithTag("Emoji");

        if (emojis != null)
        {
            emojiSprite = emojis.GetComponent<SpriteRenderer>();
            emojiSprite.enabled = false;
            emoji = emojis.GetComponent<Animator>();
            if (emoji != null)
            {
                Debug.Log("Find Emoji");
            }
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
        PuzzleWall puzzleWall = puzzleGameObject.GetComponent<PuzzleWall>();
        puzzleWall.StartCoroutine(SetEmojiTime(animationName));
        if (puzzleWall != null)
        {
            puzzleWall.soundPlayer.PlaySFX(soundPlayer.switchUsed);
            puzzleWall.useSwitch = true;
            if (puzzleWall.wall != null)
                puzzleWall.destroyObject = true;
        }
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
