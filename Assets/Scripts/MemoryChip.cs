using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryChip : MonoBehaviour
{
    public bool isTyped;
    public Dialogue dialogue;
    public GameObject particleChip;
    public string memoryTag;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("I was triggered");
        if(other.CompareTag("Player"))
        {
            Debug.Log("I compared the tag");
            DialogueTrigger.TriggerDialogue(dialogue);
            // Instantiate(particleChip, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
