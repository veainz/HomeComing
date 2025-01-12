using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBulletControlDamage : MonoBehaviour
{
    // Sát thương của viên đạn, có thể nhập từ Unity
    public int damage = 10;

    // Khi viên đạn va chạm với đối tượng khác (chỉ quái vật)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng va chạm là quái vật
        if (other.CompareTag("Enemy"))
        {
            // Lấy script HealthEnemyControl từ đối tượng quái vật
            HealthEnemyControl enemyHealth = other.GetComponent<HealthEnemyControl>();
            if (enemyHealth != null)
            {
                // Gây sát thương cho quái vật
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Enemy took {damage} damage.");
            }

            // Hủy viên đạn sau khi va chạm
            Destroy(gameObject);
        }
    }
}
