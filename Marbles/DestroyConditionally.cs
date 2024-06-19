using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyConditionally : MonoBehaviour
{
    [Header("Destroy after time")]
    public float destroyAfter;
    public bool destroyAfterEnabled;

    [Header("Destroy on click")] 
    public bool destroyOnClickEnabled;
    
    private void Start()
    {
        if (destroyAfterEnabled)
        {
            Destroy(gameObject, destroyAfter);
        }
    }
    
    private void OnMouseDown()
    {
        if (destroyOnClickEnabled)
            Destroy(gameObject);
    }

    // function to call from Unity editor (eg. button press)
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
