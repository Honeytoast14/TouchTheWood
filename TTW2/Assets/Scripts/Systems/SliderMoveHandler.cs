using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderMoveHandler : MonoBehaviour, IMoveHandler, IEndDragHandler
{
    public float step = 0.1f;           // the desired step
    bool isSelect;
    Slider slider;
    float previousSliderValue = 0f;
    GameController gameController;

    void Awake()
    {
        slider = GetComponent<Slider>();
        if (slider)
            previousSliderValue = slider.value;
    }

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (gameController.state == GameState.Setting && isSelect)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                slider.value = previousSliderValue - step;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                slider.value = previousSliderValue + step;
            }
        }

        previousSliderValue = slider.value;
    }

    public void OnMove(AxisEventData eventData)
    {
        // override the slider value using our previousSliderValue and the desired step
        if (eventData.moveDir == MoveDirection.Left)
        {
            slider.value = previousSliderValue - step;
        }

        if (eventData.moveDir == MoveDirection.Right)
        {
            slider.value = previousSliderValue + step;
        }

        // keep the slider value for future use
        previousSliderValue = slider.value;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // keep the last slider value if the slider was dragged by mouse
        previousSliderValue = slider.value;
    }
}