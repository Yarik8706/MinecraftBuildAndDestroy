using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopMechanics;
using UnityEngine;
using YG;

namespace Flatformer.GameData
{

    #region Class: ShopData
    [System.Serializable]
    public class ShopData
    {
        public List<int> purchaseAsCharacters = new List<int>();
        public List<int> secondPurchaseAsCharacters = new List<int>();

        public bool GetPurchaseAsCharacter(int index) 
            => purchaseAsCharacters.Contains(index);

        public void AddPurchaseCharacter(int index) 
            => purchaseAsCharacters.Add(index);
        
        public bool GetSecondPurchaseAsCharacter(int index) 
            => secondPurchaseAsCharacters.Contains(index);
        
        public void AddSecondPurchaseCharacter(int index) 
            => secondPurchaseAsCharacters.Add(index);

    }
    #endregion



    #region Class: Player Data
    [System.Serializable]
    public class PlayerData
    {
        public int coins;
        public int level;

        public int selectCharacterIndex;
        public int secondSelectCharacterIndex;

        // Coin
        public int GetCoin
        {
            get { return coins; }
        }
        public void AddCoins(int coin) 
            => coins += coin;

        public void SpendCoins(int coin) 
            => coins -= coin;

        public bool CanSpenCoin(int coin) 
            => this.coins >= coin;

        // Level
        public int GetLevel()
        {
            return level;
        }

        public void AddLevel(int value) 
            => level += value;

        public void SetSelectChracter(int index) 
            => selectCharacterIndex = index;

        public int GetSelectCharacter() 
            => selectCharacterIndex;
        
        public void SetSecondSelectChracter(int index) 
            => secondSelectCharacterIndex = index;
        
        public int GetSecondSelectCharacter() 
            => secondSelectCharacterIndex;

    }
    #endregion

    public static class GameDataManager
    {
        private static PlayerData _playerData;
        private static ShopData _shopData;

        
        private const string PLAYER_DATA = "player_data";
        private const string SHOP_DATA = "shop_data";

        public static void InitData()
        {
            _playerData = JsonUtility.FromJson<PlayerData>(YandexGame.savesData.playerData);

            _shopData = JsonUtility.FromJson<ShopData>(YandexGame.savesData.shopData);
            if (_playerData == null)
            {
                int currentCoin = 50;
                int currentLevel = 0;
                int currentSkin = 0;
                _playerData = new PlayerData
                {
                    coins = currentCoin,
                    level = currentLevel,
                    selectCharacterIndex = currentSkin,
                    secondSelectCharacterIndex = currentSkin,
                };
                SavePlayerData();
            }
            if(_shopData == null)
            {
                int curreentIndex = 0;
                _shopData = new ShopData
                {
                    purchaseAsCharacters = new List<int> { curreentIndex },
                    secondPurchaseAsCharacters = new List<int> { curreentIndex}
                };
                SaveShopData();
            }
            MusicManager.instance.InitData();
        }

        private static void SavePlayerData()
        {
            var data = JsonUtility.ToJson(_playerData);
            YandexGame.savesData.playerData = data;
            YandexGame.SaveProgress();
        }

        private static void SaveShopData()
        {
            var data = JsonUtility.ToJson(_shopData);
            YandexGame.savesData.shopData = data;
            YandexGame.SaveProgress();
        }

        // Handle Player Data
        public static void AddCoin(int coin)
        {
            YandexGame.savesData.allMoney += coin;
            YandexGame.NewLeaderboardScores("Score", YandexGame.savesData.allMoney);
            YandexGame.GetLeaderboard("Score",
                Int32.MaxValue, Int32.MaxValue, Int32.MaxValue, "nonePhoto");
            _playerData.AddCoins(coin);
            SavePlayerData();
        }

        public static void SpendCoin(int coin)
        {
            _playerData.SpendCoins(coin) ;
            SavePlayerData() ;
        }
        
        public static int GetCoin() 
            => _playerData.GetCoin;
        public static bool CanSpenCoin(int value) 
            => _playerData.CanSpenCoin(value);



        public static int GetLevel() 
            => _playerData.GetLevel();

        public static void AddLevel(int value)
        {
            YandexMetrica.Send("Level" + _playerData.GetLevel());
            _playerData.AddLevel(value);
            SavePlayerData();
        }

        //Handle Shop
        
        public static void SetCharacterIndex(int index, CharacterSkinType characterSkinType = CharacterSkinType.Target)
        {
            switch (characterSkinType)
            {
                case CharacterSkinType.Target:
                    _playerData.SetSelectChracter(index);
                    break;
                case CharacterSkinType.PlayerHero:
                    _playerData.SetSecondSelectChracter(index);
                    break;
                default:
                    _playerData.SetSelectChracter(index);
                    break;
            }
            _playerData.SetSelectChracter(index);
            SavePlayerData() ;
        }

        public static int GetCharacterIndex(CharacterSkinType characterSkinType = CharacterSkinType.Target)
        {
            switch (characterSkinType)
            {
                case CharacterSkinType.Target:
                    return _playerData.GetSelectCharacter();
                case CharacterSkinType.PlayerHero:
                    return _playerData.GetSecondSelectCharacter();
                default:
                    return _playerData.GetSelectCharacter();
            }
        }

        public static bool GetPurchaseAsCharacter(int index, CharacterSkinType characterSkinType = CharacterSkinType.Target)
        {
            switch (characterSkinType)
            {
                case CharacterSkinType.Target:
                    return _shopData.GetPurchaseAsCharacter(index);
                case CharacterSkinType.PlayerHero:
                    return _shopData.GetSecondPurchaseAsCharacter(index);
                default:
                    return _shopData.GetPurchaseAsCharacter(index);
            }
        }

        public static void AddPurchaseCharacter(int index, CharacterSkinType characterSkinType = CharacterSkinType.Target)
        {
            switch (characterSkinType)
            {
                case CharacterSkinType.Target:
                    _shopData.AddPurchaseCharacter(index);
                    break;
                case CharacterSkinType.PlayerHero:
                    _shopData.AddSecondPurchaseCharacter(index);
                    break;
            }
            SaveShopData();
        }
    }
}
