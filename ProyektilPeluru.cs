using UnityEngine;

public class ProyektilPeluru : MonoBehaviour
{
    public float kecepatan = 18f;
    public float damage = 20f;
    private Transform target;
    private Transform grupBatasMap;
    private bool sudahKena = false;

    private void Start()
    {
        GameObject objBatas = GameObject.Find("BatasMap");
        if (objBatas == null) objBatas = GameObject.Find("batasMap");
        if (objBatas != null) grupBatasMap = objBatas.transform;

        Destroy(gameObject, 4f);
    }

    public void SetTarget(Transform targetBaru, float nilaiDamage)
    {
        transform.SetParent(null);
        target = targetBaru;
        damage = nilaiDamage;
    }

    private void Update()
    {
        if (sudahKena) return;

        if (target == null || !target.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 titikBidik = target.position + Vector3.up * 1.2f;
        transform.position = Vector3.MoveTowards(transform.position, titikBidik, kecepatan * Time.deltaTime);
        transform.LookAt(titikBidik);

        // Jika sudah mencapai jarak 1.5 meter dari titik tengah target, langsung berikan damage
        if (Vector3.Distance(transform.position, titikBidik) <= 1.5f)
        {
            KenaTarget(target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sudahKena) return;

        if (target != null && (other.transform == target || other.transform.IsChildOf(target) || target.IsChildOf(other.transform)))
        {
            KenaTarget(target);
            return;
        }

        if (grupBatasMap != null && other.transform.IsChildOf(grupBatasMap))
        {
            Destroy(gameObject);
        }
    }

    private void KenaTarget(Transform objekTarget)
    {
        sudahKena = true;

        HealthSystem hp = objekTarget.GetComponent<HealthSystem>();
        if (hp == null) hp = objekTarget.GetComponentInParent<HealthSystem>();
        if (hp == null) hp = objekTarget.GetComponentInChildren<HealthSystem>();

        if (hp != null)
        {
            hp.TerimaDamage(damage);
        }

        Destroy(gameObject);
    }
}