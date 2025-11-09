using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private bool showMs = false;
    [SerializeField] private int decimals = 1;

    [Header("Update")]
    [SerializeField] private float updateInterval = 0.5f;

    private float timeLeft;
    private float fpsAccum;
    private int frames;

    void Awake()
    {
        timeLeft = updateInterval;

        if (fpsText == null)
        {
            // find existing Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                // create simple Canvas
                GameObject cgo = new GameObject("FPS Canvas");
                canvas = cgo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                cgo.AddComponent<CanvasScaler>();
                cgo.AddComponent<GraphicRaycaster>();
            }

            // create TMP text
            GameObject go = new GameObject("FPS Text", typeof(RectTransform));
            go.transform.SetParent(canvas.transform, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(10f, -10f);
            fpsText = go.AddComponent<TextMeshProUGUI>();
            fpsText.fontSize = 24;
            fpsText.color = Color.white;
            fpsText.raycastTarget = false;
        }
    }

    void Update()
    {
        float unscaledDelta = Time.unscaledDeltaTime;
        if (unscaledDelta <= 0f) return;

        timeLeft -= unscaledDelta;
        fpsAccum += 1f / unscaledDelta;
        frames++;

        if (timeLeft <= 0f)
        {
            float fps = fpsAccum / frames;
            if (fpsText != null)
            {
                if (showMs)
                {
                    float ms = 1000f / Mathf.Max(0.0001f, fps);
                    fpsText.text = $"{fps.ToString($"F{decimals}")} FPS ({ms.ToString($"F{decimals}")} ms)";
                }
                else
                {
                    fpsText.text = $"{fps.ToString($"F{decimals}")} FPS";
                }
            }
            timeLeft = updateInterval;
            fpsAccum = 0f;
            frames = 0;
        }
    }
}