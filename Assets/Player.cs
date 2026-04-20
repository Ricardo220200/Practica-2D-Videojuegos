using UnityEngine;
using UnityEngine.InputSystem; // Necesario para Unity 6

public class Player : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Rigidbody2D rb;

    [Header("Ajustes de Movimiento")]
    public float moveSpeed = 5f;
    public float jumpHeight = 10f; // Súbelo un poco si la gravedad es alta
    public bool isGround = true;

    private float movement;
    private bool facingRight = true;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Obtener movimiento usando el sistema de Unity 6
        movement = 0;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) movement = -1;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) movement = 1;

        // 2. Lógica para Girar (Flip)
        if (movement < 0f && facingRight) {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false) {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // 3. Lógica de Salto (Sistema Moderno)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGround) {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        // 4. Lógica de Animación de Correr
        if (Mathf.Abs(movement) > .1f) {
            animator.SetFloat("Run", 1f);
        } else {
            animator.SetFloat("Run", 0f);
        }

        // 5. Lógica de Ataque (Clic izquierdo)
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        // Movimiento físico
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

    void Jump()
    {
        // Limpiamos velocidad y saltamos
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detección de suelo por Tag
        if (collision.gameObject.tag == "Ground") {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }
}