using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PointerReacter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public MeshRenderer meshRenderer;
    public Color enterColor;
    public float duration = 1f;
    public Ease easingType = Ease.Linear;

    public RectTransform triangle;
    public Vector3 size = Vector3.one;

    private Color _originColor;
    private bool _isUIDisplayed = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        _originColor = meshRenderer.material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        meshRenderer.material.DOColor(enterColor, duration)
            .SetEase(easingType)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        meshRenderer.material.DOKill();
        meshRenderer.material.color = _originColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //카메라 기준으로 월드의 좌표를 스크린좌표로 변환하는 함수.
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        triangle.position = pos;
        triangle.gameObject.SetActive(true);
        triangle.DOScale(size, duration);

        _isUIDisplayed = true;
    }

    public void Close()
    {
        triangle.DOScale(Vector3.zero, duration)
            //.OnComplete(() => triangle.gameObject.SetActive(false));
            .OnComplete(UIDisable);

        _isUIDisplayed = false;
    }

    private void UIDisable()
    {
        triangle.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (triangle == null || !_isUIDisplayed)
            return;

        triangle.position = Camera.main.WorldToScreenPoint(transform.position);
    }

}
