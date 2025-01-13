using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float timeRemaining = 300f; // 5 phút (300 giây)
    bool isGameOver = false;

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            timeRemaining -= Time.deltaTime; // Giảm thời gian theo mỗi frame
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isGameOver = true; // Dừng lại khi thời gian hết
                // Thực hiện các hành động khi game kết thúc (nếu cần)
                Debug.Log("Game Over!");
            }

            // Hiển thị thời gian còn lại dưới dạng phút:giây
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
