using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    public int maxLife = 3;

    private int currentLife = 0;

    void OnEnable()
    {
        currentLife = maxLife;
    }

    public void TakeDamage(int damage)
    {
        currentLife = Mathf.Clamp(currentLife - damage, 0, maxLife);
        if(currentLife == 0)
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
