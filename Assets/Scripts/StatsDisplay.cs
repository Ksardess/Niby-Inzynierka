using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] private StatsController target; // przypisz obiekt (Player/Enemy) lub zostaw puste
    [SerializeField] private TextMeshProUGUI energyRegenText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI critText;
    [SerializeField] private TextMeshProUGUI elusionText;
    [SerializeField] private float refreshInterval = 0.25f;

    private float _timer;

    void Start()
    {
        if (target == null)
            target = GetComponent<StatsController>() ?? GetComponentInParent<StatsController>();
        UpdateUI();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < refreshInterval) return;
        _timer = 0f;
        UpdateUI();
    }

    public void SetTarget(StatsController newTarget)
    {
        target = newTarget;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (target == null)
        {
            if (energyRegenText != null) energyRegenText.text = "-";
            if (armorText != null) armorText.text = "-";
            if (critText != null) critText.text = "-";
            if (elusionText != null) elusionText.text = "-";
            return;
        }

        if (energyRegenText != null)
            energyRegenText.text = $"Energy Regen: {target.EnergyRegenAmount} / {target.RegenIntervalTicks} ticks";
        if (armorText != null)
            armorText.text = $"Armor: {target.ArmorPoints} (1 dmg per {target.ArmorPointsPerDamageReduction} armor)";
        if (critText != null)
            critText.text = $"Crit Chance: {target.CritChancePercent}%";
        if (elusionText != null)
            elusionText.text = $"Elusion: {target.ElusionPercent}%";
    }
}