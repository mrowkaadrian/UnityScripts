using StreamerBotIntegration;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public bool preventDuplicates = true;
    
    [Header("Spawn position")]
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    private GameObject _playersParent;

    private void Start()
    {
        _playersParent = GameObject.Find("Players");
        
        if (!_playersParent)
        {
            Debug.LogWarning("\"Players\" GameObject not found in scene.");
            _playersParent = new GameObject("Players");
        }
    }
    
    public void SpawnPlayer(UDPEventData eventData)
    {
        if (preventDuplicates && GameObject.Find(eventData.User))
        {
            Debug.Log($"User {eventData.User} already has a marble.");
        }
        else
        {
            GameObject createdObject = Instantiate(playerPrefab, GetRandomSpawnPosition(), playerPrefab.transform.rotation, _playersParent.transform);
            SetMarbleProperties(createdObject, eventData.User, eventData.Message);
        }
    }

    private void SetMarbleProperties(GameObject marble, string username, string message)
    {
        marble.name = username;
        marble.GetComponent<SpriteRenderer>().material.color = GetRandomColor();
        SetTextAvatar(marble, message);
    }
    
    private void SetTextAvatar(GameObject marble, string requestedText)
    {
        var textComponent = marble.transform.Find("Text").GetComponent<TextMeshPro>();
        textComponent.text = requestedText.Substring(0, 2).ToUpper();
    }

    private static Color GetRandomColor()
    {
        return new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );
    }

    private Vector2 GetRandomSpawnPosition()
    {
        var x = Random.Range(minX, maxX);
        var y = Random.Range(minY, maxY);
        return new Vector2(x, y);
    }
}
