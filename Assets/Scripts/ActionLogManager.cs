using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionLogManager : MonoBehaviour
{
    [Tooltip("Jeżeli puste - spróbuje znaleźć obiekt o nazwie 'LogsPanel' w scenie")]
    [SerializeField] private Transform logContainer;
    [Tooltip("Prefab powinien zawierać TextMeshProUGUI (root lub child)")]
    [SerializeField] private GameObject logEntryPrefab;
    [SerializeField] private int maxLogs = 10;

    private readonly Queue<GameObject> _entries = new Queue<GameObject>();
    public static ActionLogManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) { Destroy(gameObject); return; }

        if (logContainer == null)
        {
            var go = GameObject.Find("LogsPanel");
            if (go != null) logContainer = go.transform;
        }
    }

    public void AddLog(string message)
    {
        if (logEntryPrefab == null || logContainer == null)
        {
            Debug.LogWarning("ActionLogManager: przypisz logEntryPrefab i/lub logContainer (LogsPanel).");
            return;
        }

        GameObject go = Instantiate(logEntryPrefab, logContainer, false);
        TMP_Text txt = go.GetComponent<TMP_Text>() ?? go.GetComponentInChildren<TMP_Text>();
        if (txt != null) txt.text = message;

        _entries.Enqueue(go);
        while (_entries.Count > maxLogs)
        {
            var old = _entries.Dequeue();
            if (old != null) Destroy(old);
        }
    }

    public void Clear()
    {
        while (_entries.Count > 0)
        {
            var go = _entries.Dequeue();
            if (go != null) Destroy(go);
        }
    }
}