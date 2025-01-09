using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonSpriteChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _clickedSprite;
    [SerializeField] private Color _clickedTextColor;
    [SerializeField] private Color _normalTextColor;
    [SerializeField] private Vector2 _textOffsetOnPress = new Vector2(0, -5f);

    private Image _buttonImage;
    private TextMeshProUGUI _buttonText;
    private Vector2 _originalTextPosition;

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if (_buttonText != null)
        {
            _originalTextPosition = _buttonText.rectTransform.anchoredPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_buttonImage != null)
            _buttonImage.sprite = _clickedSprite;

        if (_buttonText != null)
        {
            _buttonText.color = _clickedTextColor;

            _buttonText.rectTransform.anchoredPosition = _originalTextPosition + _textOffsetOnPress;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_buttonImage != null)
            _buttonImage.sprite = _normalSprite;

        if (_buttonText != null)
        {
            _buttonText.color = _normalTextColor;

            _buttonText.rectTransform.anchoredPosition = _originalTextPosition;
        }
    }
}
