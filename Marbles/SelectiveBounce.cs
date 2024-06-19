using UnityEngine;

public class SelectiveBounce : MonoBehaviour
{
    public float obstacleBounciness = 300f;
    public float marbleBounciness = 50f;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            _rb.AddForce(collision.contacts[0].normal * obstacleBounciness);
        }
        if (collision.gameObject.CompareTag("Marble"))
        {
            _rb.AddForce(collision.contacts[0].normal * marbleBounciness);
        }
    }
}
