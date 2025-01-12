using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBulletControlDamage : MonoBehaviour
{
    public int minDamage = 5; // Sát thương tối thiểu của viên đạn
    public int maxDamage = 15; // Sát thương tối đa của viên đạn

    // Đảm bảo script này chỉ thực thi khi viên đạn trúng người chơi
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu va chạm với người chơi
        if (other.CompareTag("Player"))
        {
            // Lấy script Player1 từ đối tượng người chơi
            Player1 player = other.GetComponent<Player1>();
            if (player != null)
            {
                // Gây sát thương ngẫu nhiên cho người chơi
                int damage = Random.Range(minDamage, maxDamage + 1); // Tạo giá trị sát thương ngẫu nhiên
                player.TakeDamage(damage);
                Debug.Log($"Player took {damage} damage from GreenSlimeAttack.");
            }

            // Hủy viên đạn khi va chạm
            Destroy(gameObject);
        }
    }
}
