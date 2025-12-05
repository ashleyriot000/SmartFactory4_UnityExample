using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{
    public Slider healthbar;
    public Text healthText;

    public int maxLife = 3;

    private int currentLife = 0;

    void OnEnable()
    {
        currentLife = maxLife;
        GameObject go =  GameObject.Find("HealthBar");
        if(go != null)
        {
            healthbar = go.GetComponent<Slider>();
            healthbar.value = (float)currentLife / maxLife;
        }
        go = GameObject.Find("HealthText");
        if (go != null)
        {
            healthText = go.GetComponent<Text>();
            healthText.text = $"<color=red>{currentLife}</color>/{maxLife}";
        }
    }

    public void TakeDamage(int damage)
    {
        currentLife = Mathf.Clamp(currentLife - damage, 0, maxLife);
        if(healthbar != null)
            healthbar.value = (float)currentLife / maxLife;
        
        if(healthText != null) 
            healthText.text = $"<color=red>{currentLife}</color>/{maxLife}";

        if (currentLife == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //죽는 과정을 코드로 추가해야 함.
            
        //다하면 파괴
        Destroy(gameObject);
    }
}
