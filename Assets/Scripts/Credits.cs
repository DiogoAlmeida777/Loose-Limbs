using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 40f;
    private RectTransform rectTransform;
    private bool stopped = false;

    [SerializeField] private RectTransform buttonRect; // drag your button here in Inspector

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (stopped) return;

        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        // Stop when the button's world position is close to the center of the screen
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 buttonScreenPos = RectTransformUtility.WorldToScreenPoint(null, buttonRect.position);

        if (buttonScreenPos.y >= screenCenter.y)
        {
            stopped = true;
        }
    }
}