using UnityEngine;
using DG.Tweening;


public class UIOpener : MonoBehaviour
{
    public RectTransform panel;
    public Vector3 originPos;
    public Vector3 openPos;
    public float duration = 1f;
    public Ease easingType = Ease.Linear;
    public GameObject openButton;
    public GameObject closeButton;

    public void Open()
    {
        panel.DOAnchorPos(openPos, duration)
            .SetEase(easingType)
            .OnComplete(EnableCloseButton);
    }

    public void Close()
    {
        panel.DOAnchorPos(originPos, duration)
            .SetEase(easingType)
            .OnComplete(EnableOpenButton);
    }

    private void EnableOpenButton()
    {
        openButton.SetActive(true);
    }

    private void EnableCloseButton()
    {
        closeButton.SetActive(true);
    }
}
