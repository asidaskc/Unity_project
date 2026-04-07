using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Dialogue UI + runner.
/// - Works out of the box (auto-creates a simple TMP UI if you didn't set references).
/// - Press E / Submit to advance multi-line dialogue.
/// - Can also show quick one-liners that auto-hide.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References (optional)")]
    public GameObject panel;
    public TMP_Text dialogueText;
    public TMP_Text hintText;

    [Header("Input")]
    public KeyCode advanceKey = KeyCode.E;

    [Header("Behaviour")]
    public bool pauseGameDuringDialogue = true;
    public bool autoCreateUIIfMissing = true;

    [Header("Auto UI Layout")]
    public Vector2 panelSize = new Vector2(980f, 200f);
    public Vector2 panelOffset = new Vector2(0f, 80f);
    public int fontSize = 30;
    public int hintFontSize = 18;

    private string[] lines;
    private int index;
    private bool active;
    private Coroutine oneLinerRoutine;

    private void Awake()
    {
        // Simple singleton so triggers work across scenes without extra setup.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (autoCreateUIIfMissing)
            EnsureUI();

        if (panel != null)
            panel.SetActive(false);
    }

    public static DialogueManager FindOrCreate()
    {
        if (Instance != null) return Instance;

        var found = FindObjectOfType<DialogueManager>();
        if (found != null) return found;

        var go = new GameObject("DialogueManager");
        return go.AddComponent<DialogueManager>();
    }

    private void Update()
    {
        if (!active) return;

        if (Input.GetKeyDown(advanceKey) || Input.GetButtonDown("Submit"))
            Next();
    }

    public void StartDialogue(DialogueAsset asset) => StartDialogue(asset, pauseGameDuringDialogue);

    public void StartDialogue(DialogueAsset asset, bool pauseGame)
    {
        if (asset == null || asset.lines == null || asset.lines.Length == 0) return;

        EnsureUI();
        if (panel == null || dialogueText == null) return;

        // If a one-liner is showing, cancel it and switch to full dialogue.
        if (oneLinerRoutine != null)
        {
            StopCoroutine(oneLinerRoutine);
            oneLinerRoutine = null;
        }

        lines = asset.lines;
        index = 0;
        active = true;

        panel.SetActive(true);
        dialogueText.text = lines[index];
        if (hintText != null) hintText.text = $"[{advanceKey}] continue";

        if (pauseGame)
            Time.timeScale = 0f;
    }

    public void ShowLine(string line, float duration = 2f, bool pauseGame = false)
    {
        if (string.IsNullOrEmpty(line)) return;

        EnsureUI();
        if (panel == null || dialogueText == null) return;

        if (oneLinerRoutine != null)
            StopCoroutine(oneLinerRoutine);

        oneLinerRoutine = StartCoroutine(ShowLineRoutine(line, duration, pauseGame));
    }

    public void ShowRandomLine(DialogueAsset asset, float duration = 2f, bool pauseGame = false)
    {
        if (asset == null || asset.lines == null || asset.lines.Length == 0) return;
        var line = asset.lines[Random.Range(0, asset.lines.Length)];
        ShowLine(line, duration, pauseGame);
    }

    private IEnumerator ShowLineRoutine(string line, float duration, bool pauseGame)
    {
        // Don't fight with active multi-line dialogue
        if (active) yield break;

        panel.SetActive(true);
        dialogueText.text = line;
        if (hintText != null) hintText.text = "";

        float prevTimeScale = Time.timeScale;
        if (pauseGame) Time.timeScale = 0f;

        // Use realtime so it still ticks while paused.
        yield return new WaitForSecondsRealtime(Mathf.Max(0.25f, duration));

        panel.SetActive(false);

        if (pauseGame) Time.timeScale = prevTimeScale;

        oneLinerRoutine = null;
    }

    private void Next()
    {
        index++;
        if (index >= lines.Length)
        {
            End();
            return;
        }

        dialogueText.text = lines[index];
    }

    private void End()
    {
        if (panel != null) panel.SetActive(false);
        active = false;

        if (pauseGameDuringDialogue)
            Time.timeScale = 1f;
    }

    private void EnsureUI()
    {
        if (panel != null && dialogueText != null) return;

        // Create (or find) a dedicated canvas (so we don't interfere with fade/menu canvases)
        Canvas canvas = null;
        var existingCanvasGO = GameObject.Find("DialogueCanvas");
        if (existingCanvasGO != null)
            canvas = existingCanvasGO.GetComponent<Canvas>();

        if (canvas == null)
        {
            var canvasGO = new GameObject("DialogueCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5000;

            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Panel
        var panelGO = new GameObject("DialoguePanel");
        panelGO.transform.SetParent(canvas.transform, false);

        var rt = panelGO.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchoredPosition = panelOffset;
        rt.sizeDelta = panelSize;

        var img = panelGO.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.65f);

        // Main text
        var textGO = new GameObject("DialogueText");
        textGO.transform.SetParent(panelGO.transform, false);

        var textRT = textGO.AddComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0f, 0f);
        textRT.anchorMax = new Vector2(1f, 1f);
        textRT.offsetMin = new Vector2(24f, 20f);
        textRT.offsetMax = new Vector2(-24f, -40f);

        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = fontSize;
        tmp.enableWordWrapping = true;
        tmp.text = "";
        tmp.alignment = TextAlignmentOptions.TopLeft;

        // Hint text
        var hintGO = new GameObject("HintText");
        hintGO.transform.SetParent(panelGO.transform, false);

        var hintRT = hintGO.AddComponent<RectTransform>();
        hintRT.anchorMin = new Vector2(1f, 0f);
        hintRT.anchorMax = new Vector2(1f, 0f);
        hintRT.pivot = new Vector2(1f, 0f);
        hintRT.anchoredPosition = new Vector2(-18f, 10f);
        hintRT.sizeDelta = new Vector2(260f, 40f);

        var hintTMP = hintGO.AddComponent<TextMeshProUGUI>();
        hintTMP.fontSize = hintFontSize;
        hintTMP.text = "";
        hintTMP.alignment = TextAlignmentOptions.BottomRight;
        hintTMP.alpha = 0.9f;

        panel = panelGO;
        dialogueText = tmp;
        hintText = hintTMP;

        panel.SetActive(false);
    }
}
