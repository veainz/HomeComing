using System.Collections;
using UnityEngine;

public class EnemyControlDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int minDamage = 5; // Sát thương tối thiểu
    public int maxDamage = 15; // Sát thương tối đa
    public float damageDelay = 2f; // Khoảng trễ trước khi gây sát thương (giây)

    private bool isDamaging = false; // Để tránh gây sát thương nhiều lần đồng thời

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có phải là người chơi không
        if (collision.gameObject.CompareTag("Player") && !isDamaging)
        {
            isDamaging = true;
            StartCoroutine(DealDamageWithDelay(collision.gameObject));
        }
    }

    private IEnumerator DealDamageWithDelay(GameObject player)
    {
        // Chờ khoảng thời gian trước khi gây sát thương
        yield return new WaitForSeconds(damageDelay);

        // Kiểm tra xem người chơi có còn tồn tại trong game hay không
        if (player != null)
        {
            // Lấy script Player1 để gọi phương thức TakeDamage
            Player1 playerScript = player.GetComponent<Player1>();
            if (playerScript != null)
            {
                // Gây sát thương ngẫu nhiên
                int damage = Random.Range(minDamage, maxDamage + 1);
                playerScript.TakeDamage(damage);
                Debug.Log($"Enemy dealt {damage} damage to the player.");
            }
        }

        isDamaging = false; // Cho phép gây sát thương tiếp theo
    }
}
