using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueUI;  // UI panel containing text
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    private bool playerInRange = false;

    void Start()
    {
        if (dialogueUI == null)
        {
            Debug.LogError("Dialogue UI is not assigned! Make sure to assign it in the Inspector.");
            return;
        }

        dialogueUI.SetActive(false); // Hide UI initially
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // Press 'E' to start
        {
            if (!dialogueUI.activeInHierarchy) // If dialogue is not active, start it
            {
                StartDialogue();
            }
            else if (textComponent.text == lines[index]) // If text is done, go to next line
            {
                NextLine();
            }
            else // If text is not finished, show full text instantly
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        dialogueUI.SetActive(true);
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = "";
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueUI.SetActive(false); // Hide UI when dialogue ends
        }
    }

    // Detect when player enters NPC area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered NPC trigger zone!");
            playerInRange = true;
        }
    }

    // Detect when player leaves NPC area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueUI.SetActive(false); // Hide dialogue when player leaves
        }
    }
}
