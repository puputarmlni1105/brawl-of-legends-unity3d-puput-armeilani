using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScreenController : MonoBehaviour
{
    [Header("1. Video Opening Developer")]
    [SerializeField] private GameObject video1Screen;
    [SerializeField] private VideoPlayer video1Player;

    [Header("2. Video Opening Game")]
    [SerializeField] private GameObject video2Screen;
    [SerializeField] private VideoPlayer video2Player;

    [Header("3. Video Background & Loading Bar")]
    [SerializeField] private GameObject video3Screen;
    [SerializeField] private VideoPlayer video3Player;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI teksPersentase;

    private void Start()
    {
        if (video1Screen != null) video1Screen.SetActive(false);
        if (video2Screen != null) video2Screen.SetActive(false);
        if (video3Screen != null) video3Screen.SetActive(false);

        StartCoroutine(JalankanSemuaTahap());
    }

    private IEnumerator JalankanSemuaTahap()
    {
        // --- TAHAP 1: VIDEO PUPUT ARMEILANI INTERACTIVE ---
        yield return StartCoroutine(PutarVideoTunggal(video1Screen, video1Player));

        // --- TAHAP 2: VIDEO OPENING GAME ---
        yield return StartCoroutine(PutarVideoTunggal(video2Screen, video2Player));

        // --- TAHAP 3: VIDEO LOADING (30 DETIK) + LOADING BAR SINKRON ---
        if (video3Screen != null) video3Screen.SetActive(true);

        if (video3Player != null && video3Player.clip != null)
        {
            video3Player.isLooping = false; // Matikan loop agar berhenti pas di akhir video
            video3Player.Prepare();
            while (!video3Player.isPrepared) yield return null;

            video3Player.Play();
            while (!video3Player.isPlaying) yield return null;

            // Ambil durasi asli video dalam detik (misalnya 30 detik)
            double totalDurasiVideo = video3Player.length;

            while (video3Player.isPlaying)
            {
                // Hitung persentase berdasarkan waktu video yang sedang berjalan
                float progress = Mathf.Clamp01((float)(video3Player.time / totalDurasiVideo));

                if (loadingSlider != null)
                    loadingSlider.value = progress;

                if (teksPersentase != null)
                    teksPersentase.text = "Loading... " + Mathf.RoundToInt(progress * 100f) + "%";

                yield return null;
            }
        }

        // Pastikan bar penuh 100% saat video 3 tepat berakhir
        if (loadingSlider != null) loadingSlider.value = 1f;
        if (teksPersentase != null) teksPersentase.text = "Loading... 100%";

        yield return new WaitForSeconds(0.3f);

        // Pindah otomatis ke Dashboard (MainMenu)
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator PutarVideoTunggal(GameObject layar, VideoPlayer player)
    {
        if (layar != null && player != null && player.clip != null)
        {
            layar.SetActive(true);
            player.Prepare();

            while (!player.isPrepared)
            {
                yield return null;
            }

            player.Play();

            while (!player.isPlaying)
            {
                yield return null;
            }

            while (player.isPlaying)
            {
                yield return null;
            }

            layar.SetActive(false);
        }
    }
}