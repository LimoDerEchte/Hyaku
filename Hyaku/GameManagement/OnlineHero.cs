using UnityEngine;

namespace Hyaku.GameManagement
{
    public class OnlineHero : MonoBehaviour
    {
        public Animator animator;
        public SpriteRenderer renderer;
        public SkinComponent skinManager;
        public GameObject parentObject;
        public string username;
        public int clientID;
        private static GameObject _holder;
        private Vector3 desiredPos;
        private Vector3 oldPos;
        private float transitionFrames;

        public static void InitScene()
        {
            if (GameObject.Find("OnlineHeroes") == null)
            {
                _holder = new GameObject("OnlineHeroes");
            }
        }
        
        public static OnlineHero createOnlineHero(string username, int clientId, Vector3 spawnLocation)
        {
            GameObject gameObject = AssetManager.Objects.LoadAsset<GameObject>("OnlineHero");
            gameObject = Instantiate(gameObject, _holder.transform, true);
            gameObject.transform.position = spawnLocation;
            gameObject.name = $"OnlineHero-{username}-{clientId}";
            gameObject.GetComponentInChildren<TextMesh>().text = username;
            OnlineHero hero = gameObject.AddComponent<OnlineHero>();
            hero.parentObject = gameObject;
            hero.username = username;
            hero.clientID = clientId;
            hero.animator = gameObject.GetComponentInChildren<Animator>();
            hero.renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            hero.SetDesiredPos(spawnLocation);
            return hero;
        }

        public void SetDesiredPos(Vector3 pos)
        {
            oldPos = transform.position;
            desiredPos = pos;
            transitionFrames = 0;
        }

        private void Update()
        {
            if(transitionFrames < 1F)
                transitionFrames += Time.deltaTime * 15F;
            if (transitionFrames > 1F)
                transitionFrames = 1F;
            transform.position = Vector3.LerpUnclamped(oldPos, desiredPos, transitionFrames);
        }

        private void LateUpdate()
        {
            var spriteName = renderer.sprite.name;
            if (PlayerManager.Instance.Info[clientID].HeroSkinDictionary.ContainsKey(spriteName))
                renderer.sprite = PlayerManager.Instance.Info[clientID].HeroSkinDictionary[spriteName];
        }
    }
}