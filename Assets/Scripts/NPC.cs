using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCDialouge dialougeData;
    public GameObject dialougePanel;
    public TMP_Text dialougeText, nameText;
    public AudioSource TextSound;

    int DialougeIndex;
    bool isTyping, isDialougeActive;
    

    private bool hasTriggered = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && !hasTriggered)
        {
            hasTriggered = true;
            StartDialouge();
        }
    }

    void StartDialouge()
    {
        isDialougeActive = true;
        DialougeIndex = 0;
        nameText.SetText(dialougeData.npcName);
        dialougePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (isDialougeActive && !isTyping && Input.GetKeyDown(KeyCode.F))
        {
            Nextline();
        }
    }

    void Nextline()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialougeText.SetText(dialougeData.DialougeLines[DialougeIndex]);
            isTyping = false;
        }
        else if (++DialougeIndex < dialougeData.DialougeLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialouge();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialougeText.SetText("");

        foreach (char letter in dialougeData.DialougeLines[DialougeIndex])
        {
            dialougeText.text += letter;
            TextSound.Play();
            yield return new WaitForSeconds(dialougeData.typingSpeed);
        }

        isTyping = false;

        if (dialougeData.autoProgressLines.Length > DialougeIndex && dialougeData.autoProgressLines[DialougeIndex])
        {
            yield return new WaitForSeconds(dialougeData.autoProgressDelay);
            Nextline();
        }
    }

    public void EndDialouge()
    {
        StopAllCoroutines();
        isDialougeActive = false;
        dialougeText.SetText("");
        dialougePanel.SetActive(false);
    }
}
