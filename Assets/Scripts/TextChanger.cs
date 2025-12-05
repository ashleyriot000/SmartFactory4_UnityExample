using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{
    public Text uiText;
    public Text stopText;
    public Button stopButton;


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

        uiText.text = Time.time.ToString();
    }

    public void Stop()
    {
        stopText.text = Time.time.ToString();
    }

    public void StartButton()
    {
        _isStarted = true;
    }
}
