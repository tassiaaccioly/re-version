using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public Animator anim;


    public bool typed;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Player player;

    void Start()
    {
        sentences = new Queue<string>();
        player = FindObjectOfType<Player>();
        typed = FindObjectOfType<MemoryChip>().isTyped;
    }

    public void StartDialogue (Dialogue dialogue)
    {

        anim.Play("DialogueBox_Open");

        nameText.text = dialogue.name;

        player.canMove = false;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        if(typed)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        else
        {
            dialogueText.text = sentence;
        }
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.05f);
        }
    }

    void EndDialogue()
    {

        anim.Play("DialogueBox_Close");

        player.canMove = true;
    }

}
