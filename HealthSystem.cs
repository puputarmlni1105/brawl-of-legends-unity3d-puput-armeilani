using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [Header("Identitas & Darah (HP)")]
    public string namaUnit = "Player";
    public bool adalahPihakPlayer = true;
    public float maxHP = 100f;
    public float currentHP;

    private bool sudahMati = false;

    private void Start()
    {
        currentHP = maxHP;
        Debug.Log($"[START] {namaUnit} siap bertarung dengan HP: {currentHP}/{maxHP}");
    }

    public void TerimaDamage(float jumlahDamage)
    {
        if (sudahMati) return;

        currentHP -= jumlahDamage;
        if (currentHP < 0f) currentHP = 0f;

        // Cetak informasi damage & sisa HP ke tab Console
        Debug.Log($"[DAMAGE] {namaUnit} terkena {jumlahDamage} damage! Sisa HP: {currentHP}/{maxHP}");

        if (currentHP <= 0f)
        {
            sudahMati = true;
            HandleKematian();
        }
    }

    private void HandleKematian()
    {
        Debug.Log($"[ELIMINASI] {namaUnit} telah hancur / kalah (HP: 0)!");

        // Panggil UI Hasil Game pada Canvas melalui GameResultManager
        if (adalahPihakPlayer)
        {
            Debug.Log("=== HASIL PERTANDINGAN: PLAYER KALAH! ===");
            if (GameResultManager.Instance != null)
            {
                GameResultManager.Instance.TampilkanKalah();
            }
        }
        else
        {
            Debug.Log("=== HASIL PERTANDINGAN: PLAYER MENANG! ===");
            if (GameResultManager.Instance != null)
            {
                GameResultManager.Instance.TampilkanMenang();
            }
        }

        // Hentikan waktu permainan saat menang/kalah dan munculkan kursor mouse
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameObject.SetActive(false);
    }

    // Fungsi opsional untuk tombol "Main Lagi / Restart" di UI
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}