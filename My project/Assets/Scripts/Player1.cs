using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    private bool ishealing = false;
    private GameObject healingInstance; // Biến lưu đối tượng healing
    private Animator healingAnimator;
    public Transform healPoss;

    public float healTime = 5f;
    public int heal = 30;
    public GameObject healing; 
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
        StartCoroutine(Healing());
    }
     private IEnumerator Healing()
    {
        while (true)
        {
            if (currentHealth < maxHealth && !ishealing)
            {
                ishealing = true;
                yield return new WaitForSeconds(healTime);
                if (currentHealth < maxHealth)
                {
                    StartHealing();
                    currentHealth += heal;
                    Debug.Log("Current Health: " + currentHealth);   
                    if (currentHealth >= maxHealth)
                    {  
                        Debug.Log("Reached max health");
                        StopHealing();
                        currentHealth = maxHealth;
                    }
                    healthBar.UpdateBar(currentHealth, maxHealth);
                }
                ishealing = false;
            }
            yield return null;
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
            // GameOver.GameOver();
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

     private void StartHealing()
    {
        // Nếu healingInstance chưa được tạo, tạo mới healing
        if (healingInstance == null)
        {
            healingInstance = Instantiate(healing, healPoss.position, Quaternion.identity);
            healingInstance.transform.SetParent(transform);
            healingAnimator = healingInstance.GetComponent<Animator>(); // Lưu animator cho đối tượng healing
            healingAnimator.SetBool("isHealing", true);
        }
    }

    private void StopHealing()
    {
        if (healingAnimator != null)
        {
            healingAnimator.SetBool("isHealing", false);
            Destroy(healingInstance, 1f); // Thời gian chờ để hủy healingInstance sau khi dừng hoạt ảnh
            healingInstance = null; // Reset lại healingInstance
        }
        else
        {
            Debug.LogError("healingAnimator is null, cannot stop healing.");
        }
    }
}
