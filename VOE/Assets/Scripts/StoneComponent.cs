using UnityEngine;
using UnityEngine.Assertions;
namespace voe{
    public class StoneComponent : MonoBehaviour
    {
        public void set_up_type(stone_type st){
            SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>(AssetDataBase.get_stone_file_name(st));
            Assert.IsTrue(sr.sprite);
        }
    }
}