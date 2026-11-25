using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Kontrol karakter third-person: Idle, Walk, Run, Jump.
/// Butuh: CharacterController + Animator dengan parameter
/// Speed (Float), IsGrounded (Bool), Jump (Trigger).
/// Gerak : WASD atau tombol panah
/// Lari  : Shift (kiri/kanan), Ctrl (kiri/kanan), atau Z
/// Lompat: Space
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Gerakan")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Lompat dan Gravitasi")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;

    [Header("Referensi")]
    [SerializeField] private Transform cameraTransform;   // kosong = otomatis pakai Main Camera
    [SerializeField] private Animator animator;           // kosong = otomatis cari di objek / child

    private CharacterController controller;
    private float verticalVelocity;

    private static readonly int SpeedHash      = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash       = Animator.StringToHash("Jump");

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (animator != null)
            animator.applyRootMotion = false;
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // ---------- Input ----------
        Vector2 input = Vector2.zero;

        // Gerak: WASD atau tombol panah
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    input.y += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  input.y -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input.x += 1f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  input.x -= 1f;
        input = Vector2.ClampMagnitude(input, 1f);

        // Lari: Shift (kiri/kanan), Ctrl (kiri/kanan), atau Z
        bool runHeld = kb.leftShiftKey.isPressed || kb.rightShiftKey.isPressed ||
                       kb.leftCtrlKey.isPressed  || kb.rightCtrlKey.isPressed  ||
                       kb.zKey.isPressed;

        bool hasInput  = input.sqrMagnitude > 0.01f;
        bool isRunning = hasInput && runHeld;
        bool grounded  = controller.isGrounded;

        // ---------- Arah gerak relatif kamera ----------
        Vector3 camForward = Vector3.forward;
        Vector3 camRight   = Vector3.right;
        if (cameraTransform != null)
        {
            camForward = cameraTransform.forward;
            camRight   = cameraTransform.right;
        }
        camForward.y = 0f;
        camRight.y   = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.y + camRight * input.x;
        float moveSpeed = isRunning ? runSpeed : walkSpeed;

        // ---------- Putar badan menghadap arah gerak ----------
        if (hasInput && moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // ---------- Gravitasi dan lompat ----------
        if (grounded && verticalVelocity < 0f)
            verticalVelocity = -2f;   // tekanan kecil ke bawah agar tetap menempel tanah

        if (grounded && kb.spaceKey.wasPressedThisFrame)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (animator != null) animator.SetTrigger(JumpHash);
        }

        verticalVelocity += gravity * Time.deltaTime;

        // ---------- Gerakkan karakter ----------
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        // ---------- Update Animator ----------
        if (animator == null) return;

        // 0 = Idle, 0.5 = Walk, 1 = Run (tanpa damping agar cocok dengan threshold transition)
        float animSpeed = 0f;
        if (hasInput)
            animSpeed = isRunning ? 1f : 0.5f;

        animator.SetFloat(SpeedHash, animSpeed);
        animator.SetBool(IsGroundedHash, controller.isGrounded);
    }
}