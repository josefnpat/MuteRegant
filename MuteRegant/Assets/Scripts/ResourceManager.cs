using System;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private long _money = 10;
    public long Money { get { return _money; } }

    [SerializeField]
    public Gradient _cantAffordGradient;

    [SerializeField]
    private TMP_Text _moneyText;
    private static float _cantAffordT = 0.5f;
    private float _cantAffordDt = _cantAffordT;

    public void Start()
    {
        UpdateVisuals();
    }

    public void Update()
    {
        _cantAffordDt = Mathf.Min(_cantAffordT, _cantAffordDt + Time.deltaTime);
        _moneyText.color = _cantAffordGradient.Evaluate(_cantAffordDt/_cantAffordT);
    }

    private void UpdateVisuals()
    {
        _moneyText.text = $"GOLD: {ToKiloFormat(_money)}";
    }

    public void AddMoney(long money)
    {
        _money = Math.Max(0, _money + money);
        UpdateVisuals();
    }

    public void RemoveMoney(long cost)
    {
        if (cost > _money)
        {
            _money = 0;
        }
        else
        {
            _money = _money - cost;
        }
    }

    public bool CanAfford(long amountToSpend)
    {
        bool canAfford = _money >= amountToSpend;
        if (!canAfford)
        {
            _cantAffordDt = 0;
        }
        return canAfford;
    }

    public static string ToKiloFormat(long num)
    {
        return num switch
        {
            >= 1000000000000 => (num / 1000000000000D).ToString("0.#T"),
            >= 100000000000 => (num / 1000000000000D).ToString("0.##T"),
            >= 10000000000 => (num / 1000000000D).ToString("0.#B"),
            >= 1000000000 => (num / 1000000000D).ToString("0.##B"),
            >= 100000000 => (num / 1000000D).ToString("0.#M"),
            >= 1000000 => (num / 1000000D).ToString("0.##M"),
            >= 100000 => (num / 1000D).ToString("0.#k"),
            >= 10000 => (num / 1000D).ToString("0.##k"),
            _ => num.ToString("#,0")
        };
    }

}
