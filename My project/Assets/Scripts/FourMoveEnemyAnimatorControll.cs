using UnityEngine;

public class FourMoveEnemyAnimatorControll : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed = 2f;

    // Thêm biến kiểm tra trạng thái chết
    public bool isDead = false;

    // Lấy các component cần thiết
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Cập nhật trong mỗi frame
    private void Update()
    {
        // Kiểm tra xem quái vật có chết không
        if (isDead)
        {
            // Khi quái vật chết, chuyển sang animation "Die"
            animator.SetBool("isDead", true);

            // Vô hiệu hóa mọi chuyển động vật lý (dừng di chuyển)
            rb.velocity = Vector2.zero; // Dừng mọi chuyển động
            rb.isKinematic = true; // Vô hiệu hóa vật lý khi quái vật chết

            return; // Dừng tất cả các hoạt động di chuyển
        }

        // Cập nhật các trạng thái animation khác khi quái vật chưa chết
        float velocityX = rb.velocity.x;

        // Nếu có di chuyển
        if (velocityX != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(velocityX)); // Chuyển sang animation walk
            if (velocityX > 0)
            {
                // Quái vật di chuyển sang phải, lật sang phải
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (velocityX < 0)
            {
                // Quái vật di chuyển sang trái, lật sang trái
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0); // Quái vật đứng yên, chuyển sang animation idle
        }
    }

    // Hàm này sẽ được gọi từ bên ngoài (ví dụ khi quái vật bị chết)
    public void Die()
    {
        isDead = true;
    }
}
