using System;
using UnityEngine;

namespace ShopMechanics
{
    public class ShopSkinControl : MonoBehaviour
    {
        [SerializeField] private Texture[] victoryZoneSkinTextures;
        [SerializeField] private Texture[] playerHeroSkinTextures;
        [SerializeField] private Material victoryZoneSkinMaterial;
        [SerializeField] private Material playerHeroSkinMaterial;
        [SerializeField] private MeshRenderer _skinnedMeshRenderer;

        public void Init()
        {
            ChangeSkin(0, CharacterSkinType.PlayerHero);
            ChangeSkin(0);
        }

        public void ChangeSkin(int index, CharacterSkinType characterSkinType = CharacterSkinType.Target)
        {
            switch (characterSkinType)
            {
                case CharacterSkinType.Target:
                    _skinnedMeshRenderer.material = victoryZoneSkinMaterial;
                    victoryZoneSkinMaterial.SetTexture("_MainTex", victoryZoneSkinTextures[index]);
                    break;
                case CharacterSkinType.PlayerHero:
                    _skinnedMeshRenderer.material = playerHeroSkinMaterial;
                    playerHeroSkinMaterial.SetTexture("_MainTex", playerHeroSkinTextures[index]);
                    break;
                default:
                    _skinnedMeshRenderer.material = victoryZoneSkinMaterial;
                    victoryZoneSkinMaterial.SetTexture("_MainTex", victoryZoneSkinTextures[index]);
                    break;
            }
        }
    }
}