using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ADCEnemyAI : MonoBehaviour
{
    public bool roaming = true;
    public float moveSpeed = 5f;
    public float nextWPDistance = 0.5f;

    public Seeker seeker;
    public bool updateContinuesPath = false;

    bool reachDestination = true;
    Path path;
    Coroutine moveCoroutine;

    public bool isShootable = false;
    public GameObject GreenSlimeAttack; // Prefab viên đạn
    public float bulletSpeed;
    public float timeBtwFire;
    private float fireCooldown;

    // Thêm một biến để nhập thời gian tồn tại của viên đạn
    public float bulletLifeTime = 5f; // Thời gian viên đạn tồn tại (tính bằng giây)

    // Bán kính của slime (giả sử là một hình tròn)
    public float slimeRadius = 0.5f;

    // Thêm biến isDead để kiểm tra trạng thái quái vật chết
    private bool isDead = false;

    private Animator animator; // Thêm Animator để quản lý animation

    private void Start()
    {
        animator = GetComponent<Animator>(); // Lấy Animator của quái vật
        Initialize(); // Gọi phương thức Initialize khi quái vật được sinh ra
        InvokeRepeating("CalculatePath", 0f, 0.2f);
    }

    private void Update()
    {
        if (isDead) return; // Nếu quái vật đã chết, không thực hiện logic tiếp theo

        fireCooldown -= Time.deltaTime;

        if (fireCooldown < 0)
        {
            fireCooldown = timeBtwFire;
            // Shoot
            EnemyFireBullet();
        }
    }

    // Phương thức khởi tạo
    public void Initialize()
    {
        isDead = false; // Đảm bảo quái vật bắt đầu sống
        if (animator != null)
        {
            // Thiết lập animation mặc định
            animator.SetBool("isDead", false); // Giả sử "isDead" là một bool trong Animator để quản lý animation chết
        }
    }

    void EnemyFireBullet()
    {
        if (GreenSlimeAttack == null)
        {
            Debug.LogError("GreenSlimeAttack prefab is not assigned!");
            return; // Dừng lại nếu prefab không được gán
        }

        // Tính toán điểm bắn ra từ rìa của slime (từ ngoài rìa của slime)
        Vector3 firePosition = transform.position + (Vector3.right * slimeRadius); // Giả sử bắn ra từ phía phải slime
        var bulletTmp = Instantiate(GreenSlimeAttack, firePosition, Quaternion.identity);

        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        Vector3 playerPos = FindObjectOfType<Player1>().transform.position;
        Vector3 direction = playerPos - transform.position;
        rb.AddForce(direction.normalized * bulletSpeed, ForceMode2D.Impulse);

        // Hủy viên đạn sau thời gian bulletLifeTime
        Destroy(bulletTmp, bulletLifeTime);
    }

    void CalculatePath()
    {
        if (seeker == null || isDead) return; // Nếu quái vật chết, không tính đường đi

        Vector2 target = FindTarget();

        if (seeker.IsDone() && (reachDestination || updateContinuesPath))
            seeker.StartPath(transform.position, target, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (p.error || isDead) return; // Nếu quái vật chết, không thực hiện gì thêm
        path = p;
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (path == null || path.vectorPath.Count == 0 || isDead) return; // Nếu quái vật chết, không di chuyển

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }

    // Đây là Coroutine sử dụng IEnumerator
    IEnumerator MoveToTargetCoroutine()
    {
        int currentWP = 0;
        reachDestination = false;

        while (currentWP < path.vectorPath.Count && !isDead)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWP], moveSpeed * Time.deltaTime);

            // Kiểm tra hướng di chuyển và lật quái vật
            if (direction.x > 0)  // Nếu di chuyển sang phải
            {
                transform.localScale = new Vector3(1, 1, 1); // Quái vật nhìn sang phải
            }
            else if (direction.x < 0)  // Nếu di chuyển sang trái
            {
                transform.localScale = new Vector3(-1, 1, 1); // Quái vật nhìn sang trái
            }

            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWP]);
            if (distance < nextWPDistance)
                currentWP++;

            yield return null;
        }

        reachDestination = true;
    }

    Vector2 FindTarget()
    {
        if (isDead) return transform.position; // Nếu quái vật chết, không tìm kiếm mục tiêu

        Player1 player = FindObjectOfType<Player1>();
        if (player == null)
        {
            Debug.LogError("Player1 not found!");
            return transform.position; // Giữ nguyên vị trí nếu không tìm thấy Player1
        }

        Vector2 playerPos = player.transform.position;

        if (roaming)
        {
            // Tạo tọa độ ngẫu nhiên trong một vùng xung quanh người chơi
            float randomDistance = Random.Range(5f, 10f); // Khoảng cách ngẫu nhiên từ người chơi
            float randomAngle = Random.Range(0f, 360f);   // Góc ngẫu nhiên (độ)
            Vector2 randomOffset = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            ) * randomDistance;

            return playerPos + randomOffset;
        }
        else
        {
            // Nhắm thẳng vào người chơi
            return playerPos;
        }
    }

    // Hàm gọi từ bên ngoài để xác nhận quái vật đã chết
    public void Die()
    {
        if (isDead) return; // Tránh kích hoạt lại nếu đã chết rồi
        isDead = true;
        animator.SetTrigger("Die"); // Kích hoạt animation chết
    }

    // Hàm vô hiệu hóa AI khi quái vật chết
    public void DisableAI()
    {
        isDead = true;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine); // Dừng mọi coroutine di chuyển
    }
}
