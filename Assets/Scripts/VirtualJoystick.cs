using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10, 150)]
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput;

    [SerializeField]
    private PlayerController controller;
    
    public enum JoystickType { Move, Rotate }
    public JoystickType joystickType;
#if UNITY_EDITOR
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    { 
    }

#elif UNITY_ANDROID
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        switch (joystickType)
        {
            case JoystickType.Move:
                break;
        }
        isInput = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        switch (joystickType)
        {
            case JoystickType.Move:
                controller.Move(Vector2.zero);
                break;
        }
    }

    private void ControlJoystickLever(PointerEventData eventData)
    {
        Vector2 inputPos = Vector2.zero;
        switch (joystickType)
        {
            case JoystickType.Move:
                inputPos = eventData.position-rectTransform.anchoredPosition;
                break;
            case JoystickType.Rotate:
                inputPos = eventData.position - new Vector2(rectTransform.anchoredPosition.x+Screen.width,rectTransform.anchoredPosition.y);
                break;
        }

        
        Vector2 inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;
        if(joystickType==JoystickType.Move)
        controller.inputVec = inputDirection.normalized;
    }

    private void InputControlVector()
    {
        switch (joystickType)
        {
            case JoystickType.Move:
                controller.Move(inputDirection.normalized );
                break;

            case JoystickType.Rotate:
                controller.RotateShoot(inputDirection.normalized );
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isInput)
        {
            InputControlVector();
        }
        
    }
#endif
}

