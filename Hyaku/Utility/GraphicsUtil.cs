using System.Collections.Generic;
using Hyaku.GameManagement;
using UnityEngine;
using UniverseLib;
using UniverseLib.Runtime;

namespace Hyaku.Utility
{
    public class GraphicsUtil
    {
        public static List<KeyValuePair<string, Rect>> HeroSkinRects = new List<KeyValuePair<string, Rect>>()
        {
            new KeyValuePair<string, Rect>("Default_0", new Rect(0, 60, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Jump", new Rect(12, 60, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Hug", new Rect(24, 60, 12, 12)),
            new KeyValuePair<string, Rect>("Default_3", new Rect(36, 60, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Item", new Rect(48, 60, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Damaged", new Rect(60, 60, 12, 12)),
            
            new KeyValuePair<string, Rect>("Lonk_Walk_0", new Rect(0, 48, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Walk_1", new Rect(12, 48, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Walk_2", new Rect(24, 48, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Walk_3", new Rect(36, 48, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Walk_4", new Rect(48, 48, 12, 12)),
            
            new KeyValuePair<string, Rect>("Lonk_Fly_0", new Rect(0, 36, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Fly_1", new Rect(12, 36, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_HugJump_0", new Rect(24, 36, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_HugJump_1", new Rect(36, 36, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Faceplant", new Rect(48, 36, 12, 12)),
            
            new KeyValuePair<string, Rect>("Lonk_Swim_0", new Rect(0, 24, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Swim_1", new Rect(12, 24, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Swim_2", new Rect(24, 24, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Swim_3", new Rect(36, 24, 12, 12)),
            
            new KeyValuePair<string, Rect>("Lonk_Half", new Rect(0, 12, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Otherhalf", new Rect(12, 12, 12, 12)),
            
            new KeyValuePair<string, Rect>("Lonk_Fish", new Rect(0, 0, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Code_0", new Rect(12, 0, 12, 12)),
            new KeyValuePair<string, Rect>("Lonk_Code_1", new Rect(24, 0, 12, 12))
        };

        public static bool TryGetPlayerTexture(out Texture2D texture2D)
        {
            texture2D = GetPlayerTexture();
            if (texture2D != null)
                return true;
            return false;
        }

        public static Texture2D GetPlayerTexture()
        {
            Hero h = Hero.instance;
            if (h.animator != null && h.animator.gameObject.TryGetComponent(out SpriteRenderer s))
                if (s.sprite != null && s.sprite.texture != null)
                {
                    var sprite = s.sprite;
                    return TextureHelper.CopyTexture(sprite.texture, new Rect(0, 0, sprite.texture.width, sprite.texture.height));
                }

            return null;
        }

        public static void WriteToSkinDict(PlayerManager.PlayerInfo hero, Texture2D tex)
        {
            if(tex.width == 0 || tex.height == 0)
                return;
            hero.HeroSkinDictionary.Clear();
            foreach (KeyValuePair<string, Rect> pair in HeroSkinRects)
            {
                hero.HeroSkinDictionary.Add(pair.Key, Sprite.Create(tex, pair.Value, new Vector2(0.5f,0.5f), 100));
            }
        }
    }
}