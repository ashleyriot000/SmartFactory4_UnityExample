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
        triangle.gameObject.SetActive(true);
        triangle.DOScale(size, duration);
    }

    public void Close()
    {
        triangle.DOScale(Vector3.zero, duration)
            .OnComplete(() => triangle.gameObject.SetActive(false));
    }

    //public Color enterColor;
    //public MeshRenderer meshRenderer;
    //public float duration = 1f;
    //public Ease easingType = Ease.Linear;

    //private Color originColor;

    //private void Start()
    //{
    //    meshRenderer = GetComponent<MeshRenderer>();
    //    originColor = meshRenderer.material.color;
    //}


    //Tween _currentTween;
    //public void OnPointerClick(PointerEventData eventData)
    //{

    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    meshRenderer.material.DOColor(enterColor, duration)
    //        .SetEase(easingType)
    //        .SetLoops(-1, LoopType.Yoyo);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    meshRenderer.material.DOKill();
    //    meshRenderer.material.color = originColor;
    //}

}
