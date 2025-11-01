using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private Slider energySlider;
    [SerializeField] private TextMeshProUGUI energyText; // przypisz w Inspectorze

    [Header("Regen")]
    [SerializeField] private int energyRegenAmount = 5;      // ile energii regeneruje
    [SerializeField] private int regenIntervalTicks = 2;     // co ile ticków regeneracja

    private int currentEnergy;
    private bool attackedThisTick = false; // ustawiane przez gracza, jeśli wykonał atak w tej turze

    public int CurrentEnergy => currentEnergy;

    void Awake()
    {
        currentEnergy = maxEnergy;
        UpdateSlider();
    }

    public bool TryUseEnergy(int amount)
    {
        if (currentEnergy < amount) return false;
        currentEnergy = Mathf.Max(0, currentEnergy - amount);
        UpdateSlider();
        return true;
    }

    public void AddEnergy(int amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
            energySlider.value = currentEnergy;
        }

        if (energyText != null)
        {
            energyText.text = $"{currentEnergy}/{maxEnergy}";
        }
    }

    // opcjonalnie przypięcie slidera w runtime
    public void SetSlider(Slider slider)
    {
        energySlider = slider;
        UpdateSlider();
    }

    // Wywoływane przez Player po wykonaniu udanego ataku
    public void MarkAttackedThisTurn()
    {
        attackedThisTick = true;
    }

    // Wywoływane przez GridManager przy każdym Tick() (przekaż numer ticka)
    public void OnGridTick(int currentTick)
    {
        if (regenIntervalTicks > 0 && currentTick % regenIntervalTicks == 0 && !attackedThisTick)
        {
            AddEnergy(energyRegenAmount);
        }

        // reset flagi ataku po obsłużeniu Tick'a
        attackedThisTick = false;
    }
}