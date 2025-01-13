using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDirectionControl : MonoBehaviour
{
    private Vector2 direction;  // Hướng của viên đạn
    private Rigidbody2D rb;     // Rigidbody2D của viên đạn

    // Hàm này sẽ được gọi khi viên đạn được bắn ra
    public void Initialize(Vector2 fireDirection)
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D
        direction = fireDirection.normalized;  // Chuẩn hóa hướng
        RotateBullet(); // Quay viên đạn
    }

    // Quay viên đạn theo hướng
    void RotateBullet()
    {
        // Tính góc quay từ hướng
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Áp dụng góc quay
    }

    void Update()
    {
        // Có thể thực hiện thêm logic khác nếu cần
    }
}
