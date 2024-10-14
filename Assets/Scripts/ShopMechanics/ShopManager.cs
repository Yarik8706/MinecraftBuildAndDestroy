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
        [SerializeField] private AudioClip _purcharAudio;

        [Header("UI elements")] 
        [SerializeField] private ShopItemsGenerator[] _generators;
        [SerializeField] private ChangeCharacterShop _changeCharacterShop;
        [SerializeField] private ShopSkinControl _shopSkinControl;
        [SerializeField] private GameObject _shopUI;
        [SerializeField] private Button _closeShopButton;
        [SerializeField] private TextMeshProUGUI _noEnoughCoinsText;
        
        private int _newItemIndex;
        private int _preousItemIndex;
        private int _purchaseItemIndex;
        
        public static readonly MultiText UnlockLevelText = new ("Разблокируется на уровне ", "Unlock At Level ");
        public static readonly MultiText OpenOnText = new("Требуеться дней заходить: ", "Days required to get it: ");

        public static ShopManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnCompleteShopAds;
            CharacterItem.BuySkinEvent.AddListener(OnPurchaseItem);
            CharacterItem.SelectSkinEvent.AddListener(OnSelectItem);
        }
        
        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnCompleteShopAds;
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

                    SoundManager.Instance.PlayAudioSound(_purcharAudio);
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

        private static void AnimationShakeItem(Transform transform)
        {
            transform.DOComplete();
            transform.DOShakePosition(1f, new Vector3(10f, 0, 0), 10, 0).SetEase(Ease.Linear);
        }

        private void AddEvents()
        {
            _closeShopButton.onClick.RemoveAllListeners();
            _closeShopButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayAudioSound(SoundManager.Instance.buttonAudio);
                GameManager.Instance.ReplayGame();
                EventDispatcherExtension.PostEvent(EventID.IsPlayGame, true);
                EventDispatcherExtension.PostEvent(EventID.Home);
                CloseShop();
            });
        }

        private void CloseShop() 
            => _shopUI.SetActive(false);
    }
}
