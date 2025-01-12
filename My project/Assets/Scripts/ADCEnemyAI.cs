using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ADCEnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool roaming = true;
    public float moveSpeed = 5f;
    public float nextWPDistance = 0.5f;

    [Header("Pathfinding Settings")]
    public Seeker seeker;
    public bool updateContinuesPath = false;

    private bool reachDestination = true;
    private Path path;
    private Coroutine moveCoroutine;

    [Header("Shooting Settings")]
    public bool isShootable = false;
    public GameObject GreenSlimeAttack; // Prefab viên đạn
    public float bulletSpeed = 10f;
    public float timeBtwFire = 1f;
    private float fireCooldown;

    public float bulletLifeTime = 5f; // Thời gian viên đạn tồn tại
    public float slimeRadius = 0.5f; // Bán kính của slime

    [Header("State")]
    private bool isDead = false;

    private Animator animator; // Animator quản lý animation

    private void Start()
    {
        animator = GetComponent<Animator>();
        Initialize();
        InvokeRepeating("CalculatePath", 0f, 0.2f);
    }

    private void Update()
    {
        if (isDead) return; // Nếu quái vật đã chết, không làm gì cả

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0 && isShootable)
        {
            fireCooldown = timeBtwFire;
            EnemyFireBullet();
        }
    }

    // Phương thức khởi tạo
    public void Initialize()
    {
        isDead = false; // Đặt trạng thái sống
        if (animator != null)
        {
            animator.SetBool("isDead", false); // Tắt animation chết
        }
    }

    void EnemyFireBullet()
    {
        // Kiểm tra prefab GreenSlimeAttack
        if (GreenSlimeAttack == null)
        {
            Debug.LogError("GreenSlimeAttack prefab is not assigned! Please assign it in the Inspector.");
            return;
        }

        // Tính toán vị trí bắn
        Vector3 firePosition = transform.position + Vector3.right * slimeRadius; // Bắn từ rìa bên phải
        GameObject bulletTmp = Instantiate(GreenSlimeAttack, firePosition, Quaternion.identity);

        // Thêm lực cho viên đạn
        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        Player1 player = FindObjectOfType<Player1>();
        if (player == null)
        {
            Debug.LogWarning("Player1 not found! Bullet will not be fired.");
            Destroy(bulletTmp);
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);

        // Hủy viên đạn sau thời gian tồn tại
        Destroy(bulletTmp, bulletLifeTime);
    }

    void CalculatePath()
    {
        if (seeker == null || isDead) return;

        Vector2 target = FindTarget();

        if (seeker.IsDone() && (reachDestination || updateContinuesPath))
        {
            seeker.StartPath(transform.position, target, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (p.error || isDead) return;
        path = p;
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (path == null || path.vectorPath.Count == 0 || isDead) return;

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }

    IEnumerator MoveToTargetCoroutine()
    {
        int currentWP = 0;
        reachDestination = false;

        while (currentWP < path.vectorPath.Count && !isDead)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWP], moveSpeed * Time.deltaTime);

            // Lật hướng của quái vật dựa trên hướng di chuyển
            transform.localScale = new Vector3(direction.x >= 0 ? 1 : -1, 1, 1);

            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWP]);
            if (distance < nextWPDistance)
                currentWP++;

            yield return null;
        }

        reachDestination = true;
    }

    Vector2 FindTarget()
    {
        if (isDead) return transform.position;

        Player1 player = FindObjectOfType<Player1>();
        if (player == null)
        {
            Debug.LogWarning("Player1 not found! Returning current position.");
            return transform.position;
        }

        Vector2 playerPos = player.transform.position;

        if (roaming)
        {
            float randomDistance = Random.Range(5f, 10f);
            float randomAngle = Random.Range(0f, 360f);
            Vector2 randomOffset = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            ) * randomDistance;

            return playerPos + randomOffset;
        }
        else
        {
            return playerPos;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }

    public void DisableAI()
    {
        isDead = true;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }
}
