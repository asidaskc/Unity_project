using UnityEngine;

/// <summary>
/// Trigger volume that starts dialogue.
/// You can either assign a DialogueAsset directly, or provide a Resources path (e.g. "Dialogue/Intro_Level4").
/// Add a Collider2D set to IsTrigger = true on the same GameObject.
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Source")]
    public DialogueAsset dialogue;
    [Tooltip("If dialogue is null, load from Resources using this path (without the Resources/ prefix).")]
    public string dialogueResourceName = "";

    [Header("Manager")]
    public DialogueManager dialogueManager;

    [Header("Trigger Behaviour")]
    public bool triggerOnce = true;
    public bool pauseGameDuringDialogue = true;

    [Header("One-Liner Mode")]
    public bool oneLiner = false;
    public float oneLinerDuration = 2f;
    public bool pauseGameDuringOneLiner = false;

    private bool used;

    private void Awake()
    {
        if (dialogueManager == null)
            dialogueManager = DialogueManager.FindOrCreate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnce && used) return;
        if (!other.CompareTag("Player")) return;

        if (dialogueManager == null)
            dialogueManager = DialogueManager.FindOrCreate();

        DialogueAsset asset = dialogue;
        if (asset == null && !string.IsNullOrEmpty(dialogueResourceName))
            asset = Resources.Load<DialogueAsset>(dialogueResourceName);

        if (asset == null) return;

        used = true;

        if (oneLiner)
        {
            dialogueManager.ShowRandomLine(asset, oneLinerDuration, pauseGameDuringOneLiner);
        }
        else
        {
            dialogueManager.StartDialogue(asset, pauseGameDuringDialogue);
        }
    }
}
