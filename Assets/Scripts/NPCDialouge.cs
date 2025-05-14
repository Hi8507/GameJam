using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="NewNPCDialouge",menuName ="NPC Dialouge")]
public class NPCDialouge : ScriptableObject
{
    public string npcName;
    public string[] DialougeLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    public float voicePitch = 1f;
    public AudioClip voiceSound;    


}
