using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet; // Prefab viên đạn
    public Transform firePos; // Vị trí bắn
    public float TimeBtwFire = 0.3f; // Thời gian giữa các lần bắn
    public float bulletSpeed = 10f; // Tốc độ cố định của viên đạn

    private float timeBtwFire;

    private void Update()
    {
        timeBtwFire -= Time.deltaTime;

        if (Input.GetMouseButton(0) && timeBtwFire <= 0)
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        timeBtwFire = TimeBtwFire;

        // Lấy vị trí chuột trong không gian thế giới
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Đặt z = 0 vì game 2D

        // Tính toán hướng bắn
        Vector2 direction = (mousePosition - firePos.position).normalized; // Luôn chuẩn hóa để giữ hướng

        // Tạo viên đạn tại vị trí bắn
        GameObject bulletTmp = Instantiate(bullet, firePos.position, Quaternion.identity);

        // Gán vận tốc cố định cho viên đạn theo hướng
        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed; // Tốc độ cố định

        // Xoay viên đạn để nó hướng theo hướng bắn
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletTmp.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
