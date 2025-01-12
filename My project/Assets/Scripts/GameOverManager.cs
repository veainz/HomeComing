using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Màn hình hiển thị Game Over
    public GameObject blackout; // Vật thể đen mờ màn hình
    public Player1 player; // Tham chiếu tới Player1 để kiểm tra máu

    private void Update()
    {
        // Kiểm tra nếu máu của người chơi bằng 0
        if (player != null && player.GetCurrentHealth() <= 0)
        {
            TriggerGameOver();
        }
    }

    // Hàm kích hoạt Game Over
    private void TriggerGameOver()
    {
        // Hiển thị màn hình Game Over
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Tạo hiệu ứng tối màn hình
        if (blackout != null)
        {
            blackout.SetActive(true);
        }

        // Dừng toàn bộ game
        Time.timeScale = 0; // Dừng game, không thể tiếp tục
    }
}
