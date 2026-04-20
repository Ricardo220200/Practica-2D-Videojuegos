using UnityEngine;
using UnityEngine.InputSystem; // Mantenemos esto para que no te de error en Unity 6

public class Player : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator; // Nuevo: Para controlar las animaciones
    public Rigidbody2D rb;

    [Header("Ajustes del Tutorial")]
    public float moveSpeed = 7f;
    public float jumpHeight = 12f; 
    public bool isGround = true;

    private float movement;
    private bool facingRight = true;

    void Start()
    {
        // Si no arrastraste el RB al inspector, lo busca solo
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Obtener movimiento (A, D o Flechas)
        movement = 0;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) movement = -1;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) movement = 1;

        // 2. Lógica para Girar (Flip) - Como en tus capturas
        if (movement < 0f && facingRight) {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false) {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // 3. Lógica de Salto
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGround) {
            Jump();
            isGround = false;
        }

        // 4. Lógica de Animación (Run) - Agregado de tu captura
        // Si nos estamos moviendo (valor absoluto mayor a 0), activamos Run
        if (Mathf.Abs(movement) > 0f) {
            animator.SetFloat("Run", 1f);
        } else {
            animator.SetFloat("Run", 0f);
        }
    }

    private void FixedUpdate()
    {
        // Movimiento por posición
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
        }
    }
}