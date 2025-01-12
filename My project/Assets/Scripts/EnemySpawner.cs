using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab của quái vật
    public float initialSpawnInterval = 2f; // Khoảng thời gian ban đầu giữa các lần sinh quái
    public float minSpawnInterval = 0.5f; // Khoảng thời gian tối thiểu giữa các lần sinh quái
    public float spawnIntervalDecrement = 0.05f; // Giảm khoảng thời gian sinh quái vật
    public float spawnRadius = 10f; // Bán kính khu vực sinh quái
    public int maxEnemies = 30; // Số lượng tối đa quái vật có thể sinh ra trong game

    private float currentSpawnInterval; // Thời gian giữa các lần sinh quái vật
    private float timer = 0f; // Thời gian trôi qua
    private int currentEnemyCount = 0; // Số lượng quái vật hiện tại trong game

    private void Start()
    {
        // Khởi tạo giá trị thời gian sinh quái ban đầu
        currentSpawnInterval = initialSpawnInterval;
    }

    private void Update()
    {
        if (currentEnemyCount < maxEnemies)
        {
            timer -= Time.deltaTime;

            // Nếu đến thời gian sinh quái vật
            if (timer <= 0f)
            {
                SpawnEnemy(); // Sinh quái vật
                timer = currentSpawnInterval; // Đặt lại timer để tính thời gian cho lần sinh tiếp theo

                // Giảm khoảng thời gian sinh quái vật, nhưng không nhỏ hơn giá trị tối thiểu
                currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnIntervalDecrement);
            }
        }
    }

    private void SpawnEnemy()
    {
        // Sinh quái vật tại vị trí ngẫu nhiên trong phạm vi bán kính
        Vector2 spawnPosition = new Vector2(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));

        // Tạo quái vật mới tại vị trí sinh ngẫu nhiên
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Đảm bảo rằng mỗi quái vật có thể hoạt động độc lập
        ADCEnemyAI enemyAI = newEnemy.GetComponent<ADCEnemyAI>();
        if (enemyAI != null)
        {
            // Thiết lập các giá trị cần thiết cho mỗi quái vật (nếu có)
            enemyAI.Initialize(); // Khởi tạo quái vật
        }

        // Tăng số lượng quái vật hiện tại
        currentEnemyCount++;
    }
}
