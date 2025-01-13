using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab của quái vật
    public Transform player; // Vị trí người chơi
    public float minSpawnDistance = 5f; // Khoảng cách tối thiểu so với người chơi (Nhập từ Unity)
    public float maxSpawnDistance = 15f; // Khoảng cách tối đa so với người chơi (Nhập từ Unity)
    public float initialSpawnInterval = 2f; // Khoảng thời gian ban đầu giữa các lần sinh quái
    public float minSpawnInterval = 0.5f; // Khoảng thời gian tối thiểu giữa các lần sinh quái
    public float spawnIntervalDecrement = 0.05f; // Giảm khoảng thời gian sinh quái vật
    public int maxEnemies = 30; // Số lượng tối đa quái vật có thể sinh ra trong game

    public float spawnStartTime = 15f; // Thời gian bắt đầu sinh quái (tính bằng giây)
    public float spawnEndTime = 195f; // Thời gian kết thúc sinh quái (tính bằng giây)

    private float currentSpawnInterval; // Thời gian giữa các lần sinh quái vật
    private float timer = 0f; // Thời gian trôi qua
    private float gameTimer = 0f; // Thời gian trôi qua kể từ khi bắt đầu game
    private int currentEnemyCount = 0; // Số lượng quái vật hiện tại trong game

    private void Start()
    {
        // Khởi tạo giá trị thời gian sinh quái ban đầu
        currentSpawnInterval = initialSpawnInterval;
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;

        // Chỉ sinh quái vật khi trong khoảng thời gian hợp lệ
        if (gameTimer >= spawnStartTime && gameTimer <= spawnEndTime && currentEnemyCount < maxEnemies)
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
        Vector2 spawnPosition;

        // Tìm vị trí spawn hợp lệ (cách người chơi trong khoảng min-max)
        do
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
            spawnPosition = (Vector2)player.position + randomOffset;
        } while (Vector2.Distance(spawnPosition, player.position) < minSpawnDistance);

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
