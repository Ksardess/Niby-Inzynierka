using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle borderlessToggle; // opcjonalnie: borderless windowed
    [SerializeField] private Button applyButton;

    private Resolution[] availableResolutions;
    private int selectedIndex;
    private const string PREF_RES_INDEX = "res_index";
    private const string PREF_FULLSCREEN = "fullscreen";
    private const string PREF_BORDERLESS = "borderless";

    void Awake()
    {
        LoadAndPopulate();
        if (applyButton != null) applyButton.onClick.AddListener(ApplySettings);
        if (resolutionDropdown != null) resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        if (fullscreenToggle != null) fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        if (borderlessToggle != null) borderlessToggle.onValueChanged.AddListener(OnBorderlessToggled);
    }

    void Start()
    {
        // opcjonalnie: zastosuj od razu
        ApplySettings();
    }

    void Update()
    {
        // szybkie przełączanie fullscreen/windowed
        if (Input.GetKeyDown(KeyCode.F11))
        {
            ToggleFullscreen();
        }
    }

    private void LoadAndPopulate()
    {
        // Pobierz unikalne rozdzielczości (width x height)
        availableResolutions = Screen.resolutions
            .Select(r => new Resolution { width = r.width, height = r.height, refreshRate = r.refreshRate })
            .Distinct(new ResolutionComparer())
            .OrderBy(r => r.width).ThenBy(r => r.height).ToArray();

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            var options = availableResolutions.Select(r => $"{r.width} x {r.height}").ToList();
            resolutionDropdown.AddOptions(options);
        }

        // załaduj preferencje
        selectedIndex = PlayerPrefs.GetInt(PREF_RES_INDEX, availableResolutions.Length - 1);
        selectedIndex = Mathf.Clamp(selectedIndex, 0, availableResolutions.Length - 1);
        bool isFull = PlayerPrefs.GetInt(PREF_FULLSCREEN, Screen.fullScreen ? 1 : 0) == 1;
        bool isBorderless = PlayerPrefs.GetInt(PREF_BORDERLESS, 0) == 1;

        if (resolutionDropdown != null) resolutionDropdown.value = selectedIndex;
        if (fullscreenToggle != null) fullscreenToggle.isOn = isFull;
        if (borderlessToggle != null) borderlessToggle.isOn = isBorderless;
    }

    public void OnResolutionChanged(int idx)
    {
        selectedIndex = idx;
        PlayerPrefs.SetInt(PREF_RES_INDEX, selectedIndex);
    }

    public void OnFullscreenToggled(bool isOn)
    {
        PlayerPrefs.SetInt(PREF_FULLSCREEN, isOn ? 1 : 0);
    }

    public void OnBorderlessToggled(bool isOn)
    {
        PlayerPrefs.SetInt(PREF_BORDERLESS, isOn ? 1 : 0);
    }

    public void ApplySettings()
    {
        if (availableResolutions == null || availableResolutions.Length == 0) return;

        var res = availableResolutions[selectedIndex];
        bool isFull = PlayerPrefs.GetInt(PREF_FULLSCREEN, Screen.fullScreen ? 1 : 0) == 1;
        bool isBorderless = PlayerPrefs.GetInt(PREF_BORDERLESS, 0) == 1;

        FullScreenMode mode = FullScreenMode.FullScreenWindow;
        if (!isFull)
        {
            mode = isBorderless ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }
        else
        {
            // jeśli pełny ekran i borderless == true -> FullScreenWindow (borderless fullscreen)
            mode = isBorderless ? FullScreenMode.FullScreenWindow : FullScreenMode.ExclusiveFullScreen;
        }

        Screen.SetResolution(res.width, res.height, mode);
        Debug.Log($"ApplyResolution: {res.width}x{res.height} mode={mode}");
    }

    public void ToggleFullscreen()
    {
        bool current = Screen.fullScreen;
        Screen.fullScreen = !current;
        if (fullscreenToggle != null) fullscreenToggle.isOn = Screen.fullScreen;
        PlayerPrefs.SetInt(PREF_FULLSCREEN, Screen.fullScreen ? 1 : 0);
    }

    // comparer do Distinct
    private class ResolutionComparer : System.Collections.Generic.IEqualityComparer<Resolution>
    {
        public bool Equals(Resolution a, Resolution b) => a.width == b.width && a.height == b.height;
        public int GetHashCode(Resolution r) => r.width * 73856093 ^ r.height * 19349663;
    }
}