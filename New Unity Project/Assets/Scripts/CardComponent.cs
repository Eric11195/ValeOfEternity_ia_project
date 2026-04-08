
using UnityEngine;
using UnityEngine.Assertions;

using voe;

namespace voe{
    public class CardComponent : MonoBehaviour
    {
        [SerializeField]
        CardNameId card_id;
        public void set_card(CardNameId __card_id)
        {
            card_id = __card_id;
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>(AssetDataBase.get_card_file_name(card_id));
            Assert.IsTrue(sr.sprite);
            //Set card image
        }

        public CardNameId get_card_id()
        {
            return card_id;
        }
    }
}