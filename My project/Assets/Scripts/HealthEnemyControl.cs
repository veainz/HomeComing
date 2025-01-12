using System.Collections;
using UnityEngine;

public class HealthEnemyControl : MonoBehaviour
{
    // Máu tối đa của quái vật
    public int maxHealth = 100;

    // Máu hiện tại của quái vật
    private int currentHealth;

    // Tham chiếu đến thanh máu (nếu có)
    public HealthBar healthBar;

    // Tham chiếu đến script quản lý animation của quái vật
    private FourMoveEnemyAnimatorControll animatorControl;

    // Tham chiếu đến script quản lý AI của quái vật
    private ADCEnemyAI aiControl;

    // Thời gian quái vật nằm trong trạng thái chết (để xem animation trước khi biến mất)
    public float dieAnimationTime = 2f;

    private void Start()
    {
        // Khởi tạo máu ban đầu cho quái vật
        currentHealth = maxHealth;

        // Cập nhật thanh máu (nếu có)
        if (healthBar != null)
        {
            healthBar.UpdateBar(currentHealth, maxHealth);
        }

        // Lấy reference đến các component
        animatorControl = GetComponent<FourMoveEnemyAnimatorControll>();
        aiControl = GetComponent<ADCEnemyAI>();
    }

    // Hàm nhận sát thương từ viên đạn
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");

        // Đảm bảo máu không thấp hơn 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Cập nhật thanh máu (nếu có)
        if (healthBar != null)
        {
            healthBar.UpdateBar(currentHealth, maxHealth);
        }

        // Nếu máu <= 0, quái vật chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Quái vật chết
    private void Die()
    {
        Debug.Log("Enemy has died.");

        // Gọi phương thức Die trong FourMoveEnemyAnimatorControll để chuyển sang animation "Die"
        animatorControl.Die();

        // Vô hiệu hóa AI (để quái vật không di chuyển khi chết)
        aiControl.DisableAI();

        // Sau khi animation chết hoàn tất, đợi một khoảng thời gian trước khi hủy quái vật
        StartCoroutine(DestroyAfterDeath());
    }

    // Hàm hủy quái vật sau khi chết
    private IEnumerator DestroyAfterDeath()
    {
        // Đợi thời gian hiệu ứng animation chết
        yield return new WaitForSeconds(dieAnimationTime);

        // Hủy đối tượng quái vật
        Destroy(gameObject);
    }
}
