using UnityEngine;

public class UniversalCamera : MonoBehaviour
{
    [Header("Target & Jarak")]
    public Transform target;       // Tarik objek Player ke sini
    public float jarak = 5.0f;     // Jarak kamera dari player
    public float tinggi = 2.0f;    // Ketinggian kamera dari player
    
    [Header("Sensitivitas Mouse")]
    public float sensitivitasMouse = 5f;
    public float batasAtas = 60f;   // Batas menengok ke atas
    public float batasBawah = -30f;  // Batas menengok ke bawah

    private float rotasiX = 0f;
    private float rotasiY = 0f;

    void Start()
    {
        // Mengunci dan menyembunyikan kursor mouse di tengah layar saat bermain
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Membaca pergerakan mouse
        float mouseX = Input.GetAxis("Mouse X") * sensitivitasMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivitasMouse;

        rotasiY += mouseX;
        rotasiX -= mouseY;
        rotasiX = Mathf.Clamp(rotasiX, batasBawah, batasAtas); // Batasi rotasi vertikal agar tidak jungkir balik

        // Menghitung arah putaran kamera
        Quaternion rotasi = Quaternion.Euler(rotasiX, rotasiY, 0);
        
        // Menentukan posisi kamera di belakang player sesuai putaran mouse
        Vector3 posisiTarget = target.position + Vector3.up * tinggi - (rotasi * Vector3.forward * jarak);

        transform.position = posisiTarget;
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}