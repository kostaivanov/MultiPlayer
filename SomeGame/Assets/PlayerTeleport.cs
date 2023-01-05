using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTeleporter != null)
        {
            transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
        }
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.CompareTag("Teleporter"))
        {
            currentTeleporter = otherObject.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D otherObject)
    {
        if (otherObject.CompareTag("Teleporter"))
        {
            currentTeleporter = null;
        }
    }
}
