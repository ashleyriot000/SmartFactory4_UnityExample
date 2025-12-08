using UnityEngine;
using UnityEngine.UI;

public class AIUI : MonoBehaviour
{
    public Text nameText;
    public Text remainText;
    public Slider remainSlider;
    public Vector3 offset = Vector3.zero;

    private Transform _target;
    private Transform _camera;

    void Start()
    {
        //nameText = GetComponentInChildren<Text>();
        //remainSlider = GetComponentInChildren<Slider>();
        //remainText = remainSlider.GetComponentInChildren<Text>();
    }

    public void Init(Transform camera, Transform target, Transform parent)
    {
        transform.SetParent(parent);
        _camera = camera;
        _target = target;

    }

    public void ChangeMode(AIMovement.MoveState mode)
    {
        switch (mode)
        {
            case AIMovement.MoveState.Watching:
                nameText.text = $"Robot - <color=green>{mode}</color>";
                break;
            case AIMovement.MoveState.Traceing:
                nameText.text = $"Robot - <color=red>{mode}</color>";
                break;
            case AIMovement.MoveState.Returning:
                nameText.text = $"Robot - <color=orange>{mode}</color>";
                break;
            default:
                break;
        }

        //nameText.text = mode switch
        //{
        //    AIMovement.MoveState.Watching => $"Robot - <color=green>{mode}</color>",
        //    AIMovement.MoveState.Traceing => $"Robot - <color=red>{mode}</color>",
        //    AIMovement.MoveState.Returning => $"Robot - <color=orange>{mode}</color>",
        //    _ => string.Empty
        //};


        //if (mode.HasFlag(AIMovement.MoveState.Traceing))
        //    remainSlider.gameObject.SetActive(true);
        //else
        //    remainSlider.gameObject.SetActive(false);
        remainSlider.gameObject.SetActive(mode == AIMovement.MoveState.Traceing);
    }

    public void ChangeRemain(float remain,  float remainRate)
    {
        remainText.text = remain.ToString();
        remainSlider.value = remainRate;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (_camera == null || _target == null)
            return;

        transform.SetPositionAndRotation(_target.position + offset, _camera.rotation);
    }
}
