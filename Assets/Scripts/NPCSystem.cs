using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class NPCSystem : MonoBehaviour
{
    bool player_detection = false;
    public GameObject pressEPrompt;
    public GameObject dialougePanel;
    public Text dialougeText;
    public string[] dialouge;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    private Coroutine typingRoutine;


    void Start()
    {
        dialougeText.text = "";
        if (pressEPrompt != null)
        {
            pressEPrompt.SetActive(false);
        }
    }
    void Update()
    {
        if (dialougePanel == null || contButton == null || dialougeText == null) return;

        if (player_detection && !dialougePanel.activeInHierarchy)
        {
            if (pressEPrompt != null)
            {
                pressEPrompt.SetActive(true);
            }
        }
        else
        {
            if (pressEPrompt != null)
            {
                pressEPrompt.SetActive(false);
            }
        }

        if (player_detection && Input.GetKeyDown(KeyCode.E))
        {
            if (dialougePanel.activeInHierarchy)
            {
                if (dialougeText.text != dialouge[index] && typingRoutine != null)
                {
                    StopCoroutine(typingRoutine);
                    dialougeText.text = dialouge[index];
                    contButton.SetActive(true);
                }
                else
                {
                    zeroText();
                }
            }
            else
            {
                dialougePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
    }

    public void zeroText()
    {
        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
        }
        dialougeText.text = " ";
        index = 0;
    
        if (dialougePanel != null) dialougePanel.SetActive(false);
        if (contButton != null) contButton.SetActive(false);
    }

    IEnumerator Typing()
    {
        if (contButton != null) contButton.SetActive(false);
        foreach (char letter in dialouge[index].ToCharArray())
        {
            dialougeText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        if (contButton != null) contButton.SetActive(true);
        typingRoutine = null;
    }
    
    public void NextLine()
    {
        if (contButton != null)
        {
            contButton.SetActive(false);
        }
        if (index < dialouge.Length - 1)
        {
            index++;
            dialougeText.text = " ";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player_detection = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            player_detection = false;
            zeroText();
        }
    }
}
