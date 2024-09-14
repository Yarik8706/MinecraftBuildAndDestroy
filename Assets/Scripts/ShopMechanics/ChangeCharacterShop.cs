using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShopMechanics
{
    public class ChangeCharacterShop : MonoBehaviour
    {
        [SerializeField] private Button firstSelectCharasterShopButton;
        [SerializeField] private Button secondSelectCharasterShopButton;
        [SerializeField] private GameObject firstCharacterShop;
        [SerializeField] private GameObject secondCharacterShop;
        [SerializeField] private ShopItemsGenerator firstItemsGenerator;
        [SerializeField] private ShopItemsGenerator secondItemsGenerator;
        
        public void Init()
        {
            ShopManager.Instance.ShopItems = firstItemsGenerator.Items.ToArray();
            ShopManager.Instance.ActiveShopData = firstItemsGenerator.characterShopData;
            firstSelectCharasterShopButton.onClick.AddListener(() =>
            {
                firstCharacterShop.SetActive(true);
                ShopManager.Instance.DeselectActiveSelectItem();
                ShopManager.Instance.ActiveSkinType = CharacterSkinType.Target;
                ShopManager.Instance.ShopItems = firstItemsGenerator.Items.ToArray();
                ShopManager.Instance.ActiveShopData = firstItemsGenerator.characterShopData;
                ShopManager.Instance.ReselectItem();
                secondCharacterShop.SetActive(false);
            });
            secondSelectCharasterShopButton.onClick.AddListener(() =>
            {
                firstCharacterShop.SetActive(false);
                ShopManager.Instance.DeselectActiveSelectItem();
                ShopManager.Instance.ActiveSkinType = CharacterSkinType.PlayerHero;
                ShopManager.Instance.ShopItems = secondItemsGenerator.Items.ToArray();
                ShopManager.Instance.ActiveShopData = secondItemsGenerator.characterShopData;
                ShopManager.Instance.ReselectItem();
                secondCharacterShop.SetActive(true);
            });
        }
    }
}