using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public Animator anim;

    public Text nameText;
    public Text dialogueText;

    public Player player;

    void Start()
    {
        sentences = new Queue<string>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            DisplayNextSentence();
        }

    }

    public void StartDialogue (Dialogue dialogue)
    {

        anim.SetBool("isDialogueOpen", true);

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
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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

        anim.SetBool("isDialogueOpen", false);

        player.canMove = true;
    }

}
