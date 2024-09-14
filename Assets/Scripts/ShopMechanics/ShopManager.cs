using System;
using DG.Tweening;
using Flatformer.GameData;
using Platformer.Mechanics;
using Platformer.Observer;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;


namespace ShopMechanics
{
    public class ShopManager : MonoBehaviour
    {
        internal CharacterSkinType ActiveSkinType;
        internal CharacterItem[] ShopItems;
        internal CharacterShopData ActiveShopData;
        
        [Header("reference")]
        [SerializeField] private AudioClip purcharAudio;
        [Header("UI elements")] 
        [SerializeField] private ShopItemsGenerator[] _generators;
        [SerializeField] private ChangeCharacterShop _changeCharacterShop;
        [SerializeField] private ShopSkinControl _shopSkinControl;
        [SerializeField] private GameObject _shopUI;
        [SerializeField] private Button _closeShopButton;
        [SerializeField] private Button _rewardAdsButton;
        [SerializeField] private TextMeshProUGUI _noEnoughCoinsText;
        
        private int _newItemIndex;
        private int _preousItemIndex;
        private int _purchaseItemIndex;
        
        public static readonly MultiText UnlockLevelText = new ("Разблокируется на уровне ", "Unlock At Level ");

        public static ShopManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnCompleteShopAds;
            YandexGame.RewardVideoEvent += OnCompleteAds;
            this.RegisterListener(EventID.OpenShop, (param) => OpenShop());
            this.RegisterListener(EventID.SelectSkin, (param) => OnSelectItem((int)param));
            this.RegisterListener(EventID.PurchaseSkin, (param) => OnPurchaseItem((int)param));
        }
        
        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnCompleteShopAds;
            YandexGame.RewardVideoEvent -= OnCompleteAds;
            if (EventDispatcher.HasInstance())
            {
                EventDispatcher.Instance.RemoveListener(EventID.OpenShop, (param) => OpenShop());
                EventDispatcher.Instance.RemoveListener(EventID.SelectSkin, (param) => OnSelectItem((int)param));
                EventDispatcher.Instance.RemoveListener(EventID.PurchaseSkin, (param) => OnPurchaseItem((int)param));
            }
        }

        private void OnCompleteShopAds(int obj)
        {
            if(obj != (int) VideoAdsId.ShopReward) return;
            ShopItems[_purchaseItemIndex].OnCompleteAds();
        }

        public void Init()
        {
            foreach (var shopItemsGenerator in _generators)
            {
                shopItemsGenerator.Init();
            }
            _shopSkinControl.Init();
            _changeCharacterShop.Init();
            CloseShop();
            AddEvents();
            _shopSkinControl.ChangeSkin(GameDataManager.GetCharacterIndex(CharacterSkinType.PlayerHero), CharacterSkinType.PlayerHero);
            SelectItem(GameDataManager.GetCharacterIndex());
        }

        public void ReselectItem()
        {
            SelectItem(GameDataManager.GetCharacterIndex(ActiveSkinType));
        }

        public void DeselectActiveSelectItem()
        {
            ShopItems[_newItemIndex].DeSelectItem();
        }
        
        private void SetSelectedCharacter()
        {
            int index = GameDataManager.GetCharacterIndex(ActiveSkinType);
        }
        
        private void OnSelectItem(int index)
        {
            SelectItem(index);
            // Save Data
            GameDataManager.SetCharacterIndex(index, ActiveSkinType);
        }

        
        private void SelectItem(int newIndex)
        {
            _preousItemIndex = _newItemIndex;
            _newItemIndex = newIndex;
            
            CharacterItem preCharacter = ShopItems[_preousItemIndex];
            CharacterItem newCharacter = ShopItems[_newItemIndex];

            preCharacter.DeSelectItem();
            newCharacter.SelectItem();
            _shopSkinControl.ChangeSkin(newIndex, ActiveSkinType);
        }
        private void OnPurchaseItem(int index)
        {
            Debug.Log("Purchase: " + index);
            if(ActiveShopData.GetCharacter(index).isNeedAds)
            {
                _purchaseItemIndex = index;
                YandexGame.RewVideoShow((int)VideoAdsId.ShopReward);
            }
            else
            {
                Character character = ActiveShopData.GetCharacter(index);
                if(GameDataManager.CanSpenCoin(character.price))
                {
                    ShopItems[index].SetPurchaseAsCharacter();
                    ShopItems[index].OnSelectItem();

                    GameDataManager.SpendCoin(character.price);
                    GameDataManager.AddPurchaseCharacter(index, ActiveSkinType);

                    SoundManager.instance.PlayAudioSound(purcharAudio);
                    GameSharedUI.instance.UpdateCoinsTextUI();
                }
                else
                {
                    AnimationNoMoreCoinsText();
                    AnimationShakeItem(ShopItems[index].transform);
                }
            }
        }

        private void AnimationNoMoreCoinsText()
        {
            _noEnoughCoinsText.transform.DOComplete();
            _noEnoughCoinsText.DOComplete();

            _noEnoughCoinsText.transform.DOShakePosition(3f, new Vector3(5f, 0, 0), 10, 0);
            _noEnoughCoinsText.DOFade(1f, 3f).From(0f).OnComplete(() =>
            {
                _noEnoughCoinsText.DOFade(0f, 1f);
            });
        }
        public void AnimationShakeItem(Transform transform)
        {
            Debug.Log("Dotweening");
            transform.DOComplete();
            transform.DOShakePosition(1f, new Vector3(10f, 0, 0), 10, 0).SetEase(Ease.Linear);
        }

        private void AddEvents()
        {
            _closeShopButton.onClick.RemoveAllListeners();
            _closeShopButton.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
                GameManager.Instance.ReplayGame();
                this.PostEvent(EventID.IsPlayGame, true);
                this.PostEvent(EventID.Home);
                CloseShop();
            });

            _rewardAdsButton.onClick.RemoveAllListeners();
            _rewardAdsButton.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
                YandexGame.RewVideoShow((int) VideoAdsId.Reward2);
            });
        }

        private void OnCompleteAds(int id)
        {
            if(id != (int) VideoAdsId.Reward2) return;
            StartCoroutine(DelayCompleteAds(5));
        }
        private IEnumerator DelayCompleteAds(float time)
        {
            var t = time;
            while (t > 0)
            {
                yield return new WaitForEndOfFrame();
                t--;
                if (t == 0)
                    GameDataManager.AddCoin(150);
            }
            GameSharedUI.instance.UpdateCoinsTextUI();
        }
        private void FaildedAds() 
            => Debug.Log("a");

        private void OpenShop() 
            => _shopUI.SetActive(true);

        private void CloseShop() 
            => _shopUI.SetActive(false);
        
        
    }
}
