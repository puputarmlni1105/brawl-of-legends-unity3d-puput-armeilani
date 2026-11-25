using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    [Header("1. Identitas Unit")]
    public bool adalahPlayer = true;

    [Header("2. Target Lawan (Tarik Kachujin G Rosales / Ganfaul M Aure)")]
    public Transform targetKarakterLawan;
    public Transform targetTowerLawan;

    [Header("3. Peluru & Posisi Keluar")]
    public GameObject prefabBolaApi;
    public Transform titikTembak;

    [Header("4. Statistik Tembakan Otomatis")]
    public float damageTembak = 20f;
    public float jarakTembak = 15f;
    public float jedaTembak = 1.0f;

    private float timerTembak = 0f;

    private void Start()
    {
        timerTembak = jedaTembak;

        // Otomatis mencari model karakter lawan jika slot di Inspector kosong
        if (targetKarakterLawan == null)
        {
            GameObject obj = GameObject.Find(adalahPlayer ? "Ganfaul M Aure" : "Kachujin G Rosales");
            if (obj == null) obj = GameObject.Find(adalahPlayer ? "Enemy" : "Player");
            if (obj != null) targetKarakterLawan = obj.transform;
        }

        if (targetTowerLawan == null)
        {
            GameObject objTower = GameObject.Find(adalahPlayer ? "Tower_Enemy" : "Tower_Player");
            if (objTower != null) targetTowerLawan = objTower.transform;
        }
    }

    private void Update()
    {
        if (prefabBolaApi == null) return;

        timerTembak += Time.deltaTime;

        if (timerTembak >= jedaTembak)
        {
            Transform targetTerpilih = CariTargetTerdekat();
            if (targetTerpilih != null)
            {
                TembakOtomatis(targetTerpilih);
                timerTembak = 0f;
            }
        }
    }

    private Transform CariTargetTerdekat()
    {
        Transform terbaik = null;
        float jarakTerdekat = jarakTembak;

        if (targetKarakterLawan != null && targetKarakterLawan.gameObject.activeInHierarchy)
        {
            float d = Vector3.Distance(transform.position, targetKarakterLawan.position);
            if (d <= jarakTerdekat)
            {
                jarakTerdekat = d;
                terbaik = targetKarakterLawan;
            }
        }

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

    private void TembakOtomatis(Transform targetTerpilih)
    {
        Vector3 arahKeTarget = (targetTerpilih.position - transform.position).normalized;
        arahKeTarget.y = 0f;

        Vector3 posisiMuncul = titikTembak != null
            ? titikTembak.position
            : transform.position + Vector3.up * 1.5f + arahKeTarget * 2.0f;

        GameObject bola = Instantiate(prefabBolaApi, posisiMuncul, Quaternion.identity);

        ProyektilPeluru proyektil = bola.GetComponent<ProyektilPeluru>();
        if (proyektil != null)
        {
            proyektil.SetTarget(targetTerpilih, damageTembak);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, jarakTembak);
    }
}