using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Identitas Tower")]
    public bool adalahTowerPlayer = true;

    [Header("Target Lawan")]
    public Transform targetKarakterLawan;
    public Transform targetTowerLawan;

    [Header("Pengaturan Peluru")]
    public GameObject prefabPeluru;
    public Transform titikTembak;

    [Header("Statistik Tower")]
    public float jarakTembak = 45f;
    public float damageTower = 25f;
    public float jedaTembak = 1.2f;

    private float timerTembak = 0f;

    private void Start()
    {
        // Siap langsung menembak begitu lawan masuk jangkauan
        timerTembak = jedaTembak;

        // Otomatis mencari target jika slot di Inspector kosong
        if (targetKarakterLawan == null)
        {
            GameObject obj = GameObject.Find(adalahTowerPlayer ? "Enemy" : "Player");
            if (obj != null) targetKarakterLawan = obj.transform;
        }

        if (targetTowerLawan == null)
        {
            GameObject objTower = GameObject.Find(adalahTowerPlayer ? "Tower_Enemy" : "Tower_Player");
            if (objTower != null) targetTowerLawan = objTower.transform;
        }
    }

    private void Update()
    {
        if (prefabPeluru == null) return;

        timerTembak += Time.deltaTime;

        if (timerTembak >= jedaTembak)
        {
            Transform targetTerpilih = CariTargetTerdekat();
            if (targetTerpilih != null)
            {
                Tembak(targetTerpilih);
                timerTembak = 0f;
            }
        }
    }

    private Transform CariTargetTerdekat()
    {
        Transform terbaik = null;
        float jarakTerdekat = jarakTembak;

        // Prioritaskan karakter lawan yang masuk jangkauan
        if (targetKarakterLawan != null && targetKarakterLawan.gameObject.activeInHierarchy)
        {
            float d = Vector3.Distance(transform.position, targetKarakterLawan.position);
            if (d <= jarakTerdekat)
            {
                jarakTerdekat = d;
                terbaik = targetKarakterLawan;
            }
        }

        // Jika tidak ada karakter atau tower lawan lebih dekat, bidik tower lawan
        if (targetTowerLawan != null && targetTowerLawan.gameObject.activeInHierarchy)
        {
            float d = Vector3.Distance(transform.position, targetTowerLawan.position);
            if (d <= jarakTerdekat)
            {
                jarakTerdekat = d;
                terbaik = targetTowerLawan;
            }
        }

        return terbaik;
    }

    private void Tembak(Transform target)
    {
        Vector3 posisiMuncul = titikTembak != null
            ? titikTembak.position
            : transform.position + Vector3.up * 4f;

        GameObject obj = Instantiate(prefabPeluru, posisiMuncul, Quaternion.identity);
        ProyektilPeluru peluru = obj.GetComponent<ProyektilPeluru>();
        if (peluru != null)
        {
            peluru.SetTarget(target, damageTower);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jarakTembak);
    }
}