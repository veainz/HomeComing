using UnityEngine;

public class Player1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rollBoost = 5f;
    public float RollTime;
    private float rollTime;
    private bool rollOnce = false;

    private Rigidbody2D rb;
    public Animator animator;
    public Vector3 moveInput;

    // Tham chiếu thanh máu
    public HealthBar healthBar;

    // Biến cho máu
    public int maxHealth = 100;
    private int currentHealth;

    // Thêm một biến cho Collider2D
    private Collider2D playerCollider;
    public MainMenu GameOver;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        currentHealth = maxHealth;

        // Khởi tạo thanh máu
        if (healthBar != null)
        {
            healthBar.UpdateBar(currentHealth, maxHealth);
        }
    }

    private void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.position += moveInput * moveSpeed * Time.deltaTime;

        animator.SetFloat("speed", moveInput.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.Space) && rollTime <= 0)
        {
            animator.SetBool("Roll", true);
            moveSpeed += rollBoost;
            rollTime = RollTime;
            rollOnce = true;

            // Vô hiệu hóa collider để đi xuyên qua quái
            if (playerCollider != null)
                playerCollider.enabled = false;
        }

        if (rollTime <= 0 && rollOnce == true)
        {
            animator.SetBool("Roll", false);
            moveSpeed -= rollBoost;
            rollOnce = false;

            // Khôi phục lại collider sau khi hết roll
            if (playerCollider != null)
                playerCollider.enabled = true;
        }
        else
        {
            rollTime -= Time.deltaTime;
        }

        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(moveInput.x > 0 ? 1 : -1, 1, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        // Đảm bảo giá trị máu không nhỏ hơn 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Cập nhật thanh máu
        if (healthBar != null)
        {
            healthBar.UpdateBar(currentHealth, maxHealth);
        }

        // Kiểm tra nếu máu <= 0
        if (currentHealth <= 0)
        {
            Die();
            GameOver.GameOver();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Thêm logic khi nhân vật chết (nếu cần)
    }

    // Phương thức để lấy máu hiện tại
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
