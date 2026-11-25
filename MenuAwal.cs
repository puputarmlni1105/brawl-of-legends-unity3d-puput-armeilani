using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Panel CanvasGroup")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private CanvasGroup profileCanvasGroup;
    [SerializeField] private CanvasGroup aboutCanvasGroup;
    [SerializeField] private CanvasGroup settingCanvasGroup;

    [Header("Komponen Setting")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown grafikDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        TampilkanMenu();
        MuatPengaturanAwal();
    }

    private void MuatPengaturanAwal()
    {
        // Muat volume tersimpan (default 1 = 100%)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVolume;
        if (volumeSlider != null) volumeSlider.value = savedVolume;

        // Muat kualitas grafik tersimpan
        int savedQuality = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQuality);
        if (grafikDropdown != null) grafikDropdown.value = savedQuality;

        // Muat status layar penuh
        if (fullscreenToggle != null) fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void MulaiGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void BukaProfile()
    {
        AturPanel(menuCanvasGroup, false);
        AturPanel(profileCanvasGroup, true);
        AturPanel(aboutCanvasGroup, false);
        AturPanel(settingCanvasGroup, false);
    }

    public void BukaAbout()
    {
        AturPanel(menuCanvasGroup, false);
        AturPanel(profileCanvasGroup, false);
        AturPanel(aboutCanvasGroup, true);
        AturPanel(settingCanvasGroup, false);
    }

    public void BukaSetting()
    {
        AturPanel(menuCanvasGroup, false);
        AturPanel(profileCanvasGroup, false);
        AturPanel(aboutCanvasGroup, false);
        AturPanel(settingCanvasGroup, true);
    }

    public void KembaliKeMenu()
    {
        TampilkanMenu();
    }

    private void TampilkanMenu()
    {
        AturPanel(menuCanvasGroup, true);
        AturPanel(profileCanvasGroup, false);
        AturPanel(aboutCanvasGroup, false);
        AturPanel(settingCanvasGroup, false);
    }

    // Mengatur visibilitas dan kemampuan klik tanpa mematikan GameObject
    private void AturPanel(CanvasGroup cg, bool aktif)
    {
        if (cg == null) return;
        cg.alpha = aktif ? 1f : 0f;
        cg.interactable = aktif;
        cg.blocksRaycasts = aktif;
    }

    // --- FUNGSI PENGATURAN (SETTING) ---
    public void UbahVolume(float nilai)
    {
        AudioListener.volume = nilai;
        PlayerPrefs.SetFloat("MasterVolume", nilai);
    }

    public void UbahKualitasGrafik(int indeks)
    {
        QualitySettings.SetQualityLevel(indeks);
        PlayerPrefs.SetInt("QualityLevel", indeks);
    }

    public void UbahFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void KeluarGame()
    {
        Debug.Log("Game Keluar");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}