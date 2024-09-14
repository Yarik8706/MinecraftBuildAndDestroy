using Flatformer.GameData;
using TMPro;
using UnityEngine;



public class GameSharedUI : MonoBehaviour
{

    #region Singleton Class: GameSharedUI
    public static GameSharedUI instance;

    private void Awake()
    {
        if(instance == null)
        instance = this; 
    }

    #endregion





    [SerializeField] private TMP_Text[] _coinTexts;


    public void Init()
    {
        UpdateCoinsTextUI();
    }

    public void UpdateCoinsTextUI()
    {
        for (int i = 0; i < _coinTexts.Length; i++)
        {
            SetCoinsText(_coinTexts[i], GameDataManager.GetCoin());
        }
    }

    private void SetCoinsText(TMP_Text coinText, int value)
    {
        if(value >= 1000)
        {
            coinText.text = string.Format("{0}K,{1}", (value / 1000), Mathf.Round(value % 1000 / 100));
        }
        else
        {
            coinText.text = value.ToString();
        }
    }

    private int GetFirstDigitFromNumber(int number)
    {
        return int.Parse(number.ToString()[0].ToString());
    }
    
}
