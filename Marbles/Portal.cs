using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destination;
    
    private GameObject _playerObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Marble")) return;
        
        _playerObject = other.gameObject;
        _playerObject.SetActive(false);
        _playerObject.transform.position = destination.position;
        _playerObject.SetActive(true);
    }
}