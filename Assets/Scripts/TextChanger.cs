using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{
    public Text uiText;
    public Text uiTextClone;
    public Text stopText;
    public Text stopTextClone;
    public Button stopButton;

    private float _startedTime = 0f;

    private void Start()
    {
        uiText.text = gameObject.name;
        //GameObject.Find("씬 내에 존재하는 게임오브젝트의 이름") 호출하면 찾아보고
        //있으면 찾은 게임오브젝트를 반환하고 없으면 null을 반환한다.
        //stopText = GameObject.Find("StopText").GetComponent<Text>();
        GameObject go = GameObject.Find("StopText");
        if(go != null)
            stopText = go.GetComponent<Text>();

        go = GameObject.Find("StopButton");
        //TextChanger tc = FindAnyObjectByType<TextChanger>(FindObjectsInactive.Include);
        if(go != null)
        {
            stopButton = go.GetComponent<Button>();
            if(stopButton != null)
            {
                stopButton.onClick.AddListener(Stop);
            }
        }
    }

    private bool _isStarted = false;
    private void Update()
    {
        if (!_isStarted)
            return;
        
        uiTextClone.text = uiText.text = (Time.time - _startedTime).ToString();
    }

    public void Stop()
    {
        if (!_isStarted)
        {
            //stopTextClone.text = stopText.text = "0";
            return;
        }
        stopTextClone.text = stopText.text = (Time.time - _startedTime).ToString();
    }

    public void StartButton()
    {
        _isStarted = true;
        _startedTime = Time.time;
    }
}
