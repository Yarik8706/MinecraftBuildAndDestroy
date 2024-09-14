using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.Events;
using Flatformer.GameData;
using Platformer.Observer;
using YG;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FortuneWheelManager : MonoBehaviour
{
    [Header("Game Objects for some elements")]
    public Button PaidTurnButton;               // This button is showed when you can turn the wheel for coins
    public Button FreeTurnButton;               // This button is showed when you can turn the wheel for free
    public Button btnClose;
    public GameObject Circle;                   // Rotatable GameObject on scene with reward objects
    private bool _isStarted;                    // Flag that the wheel is spinning

    [Header("Params for each sector")]
    public FortuneWheelSector[] Sectors;        // All sectors objects

    private float _finalAngle;                  // The final angle is needed to calculate the reward
    private float _startAngle;                  // The first time start angle equals 0 but the next time it equals the last final angle
    private float _currentLerpRotationTime;     // Needed for spinning animation


    // Flag that player can turn the wheel for free right now
    private bool _isFreeTurnAvailable;

    private FortuneWheelSector _finalSector;
    float timeTemp;
    float timeShowAds = 20f;
    private void Awake()
    {

        // Show sector reward value in text object if it's set
        foreach (var sector in Sectors)
        {
            if (sector.ValueTextObject != null)
            {
                sector.ValueTextObject.GetComponent<Text>().text = sector.RewardValue.ToString();
            }
        }


        _isFreeTurnAvailable = CheckFreeTurn();
        timeTemp = Time.time;
    }
    public void ClosePopup()
    {
        //if (Time.time - timeTemp > timeShowAds)
        //{
        //    AdmobManager.instance.ShowInter("spin");
        //    timeTemp = Time.time;
        //}
        gameObject.SetActive(false);
        this.PostEvent(EventID.Home);
    }
   
    private void TurnWheelForFree()
    {
        TurnWheel(true);
    }
    private void TurnWheelForAds(int id)
    {
        if(id != (int) VideoAdsId.TurnWheel) return;
        TurnWheel(false);
    }
    private bool CheckFreeTurn()
    {
        if (YandexGame.savesData.freeSpin != 0)
        {
            return YandexGame.savesData.freeSpin != DateTime.Now.DayOfYear;
        }
        else
        {
            return true;
        }
    }
    private void TurnWheel(bool isFree)
    {
        _currentLerpRotationTime = 0f;

        // All sectors angles
        int[] sectorsAngles = new int[Sectors.Length];

        // Fill the necessary angles (for example if we want to have 12 sectors we need to fill the angles with 30 degrees step)
        // It's recommended to use the EVEN sectors count (2, 4, 6, 8, 10, 12, etc)
        for (int i = 1; i <= Sectors.Length; i++)
        {
            sectorsAngles[i - 1] = 360 / Sectors.Length * i;
        }

        //int cumulativeProbability = Sectors.Sum(sector => sector.Probability);

        double rndNumber = UnityEngine.Random.Range(0f, Sectors.Sum(sector => sector.Probability));

        // Calculate the propability of each sector with respect to other sectors
        float cumulativeProbability = 0;
        // Random final sector accordingly to probability
        int randomFinalAngle = sectorsAngles[0];
        _finalSector = Sectors[0];

        for (int i = 0; i < Sectors.Length; i++)
        {
            cumulativeProbability += Sectors[i].Probability;

            if (rndNumber <= cumulativeProbability)
            {
                // Choose final sector
                randomFinalAngle = sectorsAngles[i];
                _finalSector = Sectors[i];
                break;
            }
        }

        int fullTurnovers = 5;

        // Set up how many turnovers our wheel should make before stop
        _finalAngle = fullTurnovers * 360 + randomFinalAngle - 20f;

        // Stop the wheel
        _isStarted = true;
        DisableFreeTurnButton();
        DisablePaidTurnButton();
        DisableButton(btnClose);
        // Decrease money for the turn if it is not free turn
        if (!isFree)
        {

            //done spin paid
        }
        else
        {

            // Restart timer to next free turn
            SetNextFreeTime();
        }
    }
    
    // Подписываемся на событие открытия рекламы в OnEnable
    private void OnEnable() => YandexGame.RewardVideoEvent += TurnWheelForAds;
    
    private void OnDisable() => YandexGame.RewardVideoEvent -= TurnWheelForAds;

    public void TurnWheelButtonClick()
    {
        if (_isFreeTurnAvailable)
        {
            TurnWheelForFree();
        }
        else
        {
            YandexGame.RewVideoShow((int) VideoAdsId.TurnWheel);
        }
    }

    public void SetNextFreeTime()
    {
        YandexGame.savesData.freeSpin = DateTime.Now.DayOfYear;
        YandexGame.SaveProgress();
        _isFreeTurnAvailable = false;
    }

    private void ShowTurnButtons()
    {
        if (_isFreeTurnAvailable)               // If have free turn
        {
            ShowFreeTurnButton();
        }
        else
        {
            ShowPaidTurnButton();
        }
       
    }

    private void Update()
    {
        // We need to show TURN FOR FREE button or TURN FOR COINS button
        ShowTurnButtons();

        if (!_isStarted)
            return;

        // Animation time
        float maxLerpRotationTime = 4f;

        // increment animation timer once per frame
        _currentLerpRotationTime += Time.deltaTime;

        // If the end of animation
        if (_currentLerpRotationTime > maxLerpRotationTime || Circle.transform.eulerAngles.z == _finalAngle)
        {
            _currentLerpRotationTime = maxLerpRotationTime;
            _isStarted = false;
            EnablePaidTurnButton();
            EnableFreeTurnButton();
            EnableButton(btnClose);
            _startAngle = _finalAngle % 360;

            //GiveAwardByAngle ();
            _finalSector.RewardCallback.Invoke();
        }
        else
        {
            // Calculate current position using linear interpolation
            float t = _currentLerpRotationTime / maxLerpRotationTime;

            // This formulae allows to speed up at start and speed down at the end of rotation.
            // Try to change this values to customize the speed
            t = t * t * t * (t * (6f * t - 15f) + 10f);

            float angle = Mathf.Lerp(_startAngle, _finalAngle, t);
            Circle.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    /// <summary>
    /// Sample callback for giving reward (in editor each sector have Reward Callback field pointed to this method)
    /// </summary>
    /// <param name="awardCoins">Coins for user</param>
    public void RewardCoins(float awardCoins)
    {
        
        GameDataManager.AddCoin((int)awardCoins);
        GameSharedUI.instance.UpdateCoinsTextUI();
    }

    private void EnableButton(Button button)
    {
        button.interactable = true;
        //button.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
    }

    private void DisableButton(Button button)
    {
        button.interactable = false;
        //button.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
    }

    // Function for more readable calls
    private void EnableFreeTurnButton() { EnableButton(FreeTurnButton); }
    private void DisableFreeTurnButton() { DisableButton(FreeTurnButton); }
    private void EnablePaidTurnButton() { EnableButton(PaidTurnButton); }
    private void DisablePaidTurnButton() { DisableButton(PaidTurnButton); }

    private void ShowFreeTurnButton()
    {
        FreeTurnButton.gameObject.SetActive(true);
        PaidTurnButton.gameObject.SetActive(false);
    }

    private void ShowPaidTurnButton()
    {
        PaidTurnButton.gameObject.SetActive(true);
        FreeTurnButton.gameObject.SetActive(false);
    }
}

/**
 * One sector on the wheel
 */
[Serializable]
public class FortuneWheelSector : System.Object
{
    [Tooltip("Text object where value will be placed (not required)")]
    public GameObject ValueTextObject;

    [Tooltip("Value of reward")]
    public float RewardValue = 100;

    [Tooltip("Chance that this sector will be randomly selected")]
    [RangeAttribute(0, 100)]
    public float Probability = 100;

    [Tooltip("Method that will be invoked if this sector will be randomly selected")]
    public UnityEvent RewardCallback;
}