using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsController : MonoBehaviour
{
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private Slider energySlider;
    [SerializeField] private TextMeshProUGUI energyText; // przypisz w Inspectorze

    [Header("Regen")]
    [SerializeField] private int energyRegenAmount = 5;      // ile energii regeneruje
    [SerializeField] private int regenIntervalTicks = 2;     // co ile ticków regeneracja

    [Header("Armor")]
    [SerializeField, Tooltip("Ilość punktów pancerza gracza")] private int armorPoints = 0;
    [SerializeField, Tooltip("Ile punktów pancerza daje -1 do otrzymywanych obrażeń")] private int armorPointsPerDamageReduction = 2;

    // --- DODANE: Crit Chance ---
    [Header("Critical")]
    [SerializeField, Range(0f, 100f), Tooltip("Szansa na trafienie krytyczne w procentach (np. 10 = 10%)")]
    private float critChancePercent = 0f;
    public float CritChancePercent => critChancePercent;

    [Header("Combat")]
    [SerializeField, Tooltip("Podstawowe obrażenia zadawane przez gracza")]
    private int baseDamage = 25;
    public int BaseDamage => baseDamage;
    
    private int currentEnergy;
    private bool attackedThisTick = false; // ustawiane przez gracza, jeśli wykonał atak w tej turze

    public int CurrentEnergy => currentEnergy;
    public int ArmorPoints => armorPoints;
    public int ArmorPointsPerDamageReduction => Mathf.Max(1, armorPointsPerDamageReduction);

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

    // ---- Armor helpers (wywoływane przez HealthController) ----
    public void AddArmor(int amount)
    {
        armorPoints = Mathf.Max(0, armorPoints + amount);
    }

    public void SetArmor(int value)
    {
        armorPoints = Mathf.Max(0, value);
    }

    public void SetArmorPointsPerDamageReduction(int value)
    {
        armorPointsPerDamageReduction = Mathf.Max(1, value);
    }

    // Zwraca ostateczną wartość obrażeń po uwzględnieniu pancerza
    public int CalculateDamageAfterArmor(int incomingDamage)
    {
        if (armorPointsPerDamageReduction <= 0) return incomingDamage;
        int reduction = armorPoints / ArmorPointsPerDamageReduction;
        return Mathf.Max(0, incomingDamage - reduction);
    }

    // --- DODANE: krytyczne trafienie ---
    // Zwraca true jeśli rzut krytyka się powiódł
    public bool RollCrit()
    {
        if (critChancePercent <= 0f) return false;
        return Random.value < (critChancePercent / 100f);
    }
}