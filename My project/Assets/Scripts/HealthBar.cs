using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image fillBar;
    public TextMeshProUGUI valueText;

    public void UpdateBar(int currentValue, int maxValue)
    {
        // Cập nhật tỷ lệ lấp đầy
        fillBar.fillAmount = (float)currentValue / maxValue;

        // Cập nhật văn bản hiển thị % máu
        valueText.text = $"{Mathf.RoundToInt((float)currentValue / maxValue * 100)}%";

        // Debug để kiểm tra giá trị
        Debug.Log($"HealthBar Updated: {currentValue}/{maxValue} ({fillBar.fillAmount * 100}%)");
    }
}
