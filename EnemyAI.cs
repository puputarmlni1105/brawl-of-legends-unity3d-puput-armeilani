using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private Transform targetPlayer; 
    [SerializeField] private float kecepatanJalan = 3f;

    [Header("Gravitasi")]
    [SerializeField] private float gravity = -20f;

    [Header("Referensi")]
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (targetPlayer == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                targetPlayer = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        if (targetPlayer == null) return;

        bool grounded = controller.isGrounded;

        if (grounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        // Hitung jarak ke Player 1
        float jarak = Vector3.Distance(transform.position, targetPlayer.position);

        Vector3 moveDir = Vector3.zero;

        // Jika jarak lebih dari 1.5 meter, kejar player
        if (jarak > 1.5f)
        {
            Vector3 arahKePlayer = targetPlayer.position - transform.position;
            arahKePlayer.y = 0f; 
            arahKePlayer.Normalize();

            if (arahKePlayer != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(arahKePlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 15f * Time.deltaTime);
            }

            moveDir = arahKePlayer * kecepatanJalan;

            // NYALAKAN ANIMASI JALAN
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
            }
        }
        else
        {
            // JIKA SUDAH DEKAT, BERHENTI DAN DIAM
            moveDir = Vector3.zero;

            // MATIKAN ANIMASI JALAN (KEMBALI KE IDLE)
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }
        }

        moveDir.y = verticalVelocity;
        controller.Move(moveDir * Time.deltaTime);
    }
}