using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryController : MonoBehaviour
{
    //memories
    public int totalOfAbilityMemories = 0;
    public int totalOfFullMemories = 0;
    public int totalOfBrokenMemories = 0;

    private int totalOfMemories = 12;
    private string[] abilityMemories;
    private string[] fullMemories;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        abilityMemories = new string[totalOfMemories];
        fullMemories = new string[totalOfMemories];
    }

    public void addAbilityMemory(string memoryTag)
    {
        totalOfAbilityMemories++;
        Debug.Log("Total of Ability Memories: " + totalOfAbilityMemories);
        abilityMemories.SetValue(memoryTag, totalOfAbilityMemories - 1);
        Debug.Log(abilityMemories[0]);
    }

    public void addFullMemory(string memoryTag)
    {
        totalOfFullMemories++;
        Debug.Log("Total of Full Memories: " + totalOfFullMemories);
        fullMemories.SetValue(memoryTag, totalOfFullMemories - 1);
        Debug.Log(fullMemories[0]);
    }

    public void addBrokenMemory(string memoryTag)
    {

    }
}
