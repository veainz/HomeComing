using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Tạo biến lưu trữ audio source
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    // Tạo biến lưu trữ audio Clip
    public AudioClip musicClip;
    public AudioClip gunClip;
    public AudioClip winClip;
    public AudioClip loseClip;  // Thêm âm thanh thua (ví dụ)

    // Biến instance để truy cập AudioManager từ bất kỳ nơi nào trong game
    public static AudioManager instance;

    void Awake()
    {
        // Kiểm tra xem có một instance AudioManager đã tồn tại chưa
        if (instance == null)
        {
            // Nếu chưa, gán instance cho AudioManager này
            instance = this;
        }
        else if (instance != this)
        {
            // Nếu đã có instance, phá hủy đối tượng mới tạo ra (đảm bảo chỉ có một instance)
            Destroy(gameObject);
        }

        // Đảm bảo rằng AudioManager không bị hủy khi chuyển cảnh
        DontDestroyOnLoad(gameObject);

        PlayMusic();  // Phát nhạc nền ngay khi bắt đầu
    }

    // Phát nhạc nền
    public void PlayMusic()
    {
        if (musicAudioSource != null && musicClip != null)
        {
            musicAudioSource.clip = musicClip;  // Gán clip nhạc nền
            musicAudioSource.loop = true;       // Lặp lại nhạc nền
            musicAudioSource.Play();            // Bắt đầu phát nhạc
        }
        else
        {
            Debug.LogWarning("Music Audio Source hoặc Music Clip chưa được thiết lập!");
        }
    }

    // Phát âm thanh tiếng súng
    public void PlayGunSound()
    {
        if (vfxAudioSource != null && gunClip != null)
        {
            vfxAudioSource.PlayOneShot(gunClip);  // Phát âm thanh tiếng súng
        }
        else
        {
            Debug.LogWarning("VFX Audio Source hoặc Gun Clip chưa được thiết lập!");
        }
    }

    // Phát âm thanh chiến thắng
    public void PlayWinSound()
    {
        if (vfxAudioSource != null && winClip != null)
        {
            vfxAudioSource.PlayOneShot(winClip);  // Phát âm thanh chiến thắng
        }
        else
        {
            Debug.LogWarning("VFX Audio Source hoặc Win Clip chưa được thiết lập!");
        }
    }

    // Phát âm thanh thua
    public void PlayLoseSound()
    {
        if (vfxAudioSource != null && loseClip != null)
        {
            vfxAudioSource.PlayOneShot(loseClip);  // Phát âm thanh thua
        }
        else
        {
            Debug.LogWarning("VFX Audio Source hoặc Lose Clip chưa được thiết lập!");
        }
    }
}
