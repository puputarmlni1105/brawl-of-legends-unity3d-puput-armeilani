using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResultManager : MonoBehaviour
{
    public static GameResultManager Instance;

    [Header("UI Hasil Pertandingan")]
    public GameObject panelHasilGame;
    public TextMeshProUGUI teksHasil;

    private bool gameSelesai = false;

    private void Awake()
    {
        Instance = this;
        if (panelHasilGame != null)
            panelHasilGame.SetActive(false);
    }

    public void TampilkanMenang()
    {
        if (gameSelesai) return;
        gameSelesai = true;

        if (panelHasilGame != null) panelHasilGame.SetActive(true);
        if (teksHasil != null)
        {
            teksHasil.text = "MENANG!";
            teksHasil.color = Color.yellow;
        }
    }

    public void TampilkanKalah()
    {
        if (gameSelesai) return;
        gameSelesai = true;

        if (panelHasilGame != null) panelHasilGame.SetActive(true);
        if (teksHasil != null)
        {
            teksHasil.text = "KALAH!";
            teksHasil.color = Color.red;
        }
    }

    public void KembaliKeDashboard()
    {
        SceneManager.LoadScene("MainMenu");
    }
}