using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public bool facingLeft = true;
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mover al enemigo hacia la izquierda constantemente (según su orientación local)
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        // Lanzar un rayo hacia abajo para detectar el suelo
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

        // Si el rayo no toca suelo (hit == false), el enemigo debe girar
        if (hit == false && facingLeft) 
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = false;
        }
        else if (hit == false && facingLeft == false) 
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
        }
    }

    // Dibujar el rayo en el editor para poder ajustarlo visualmente
    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
    }
}