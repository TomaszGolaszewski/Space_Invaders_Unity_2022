using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool leftPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        leftPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        leftPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        leftPressed = false;
    }
}
