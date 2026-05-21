
using UnityEngine;
using UnityEngine.Assertions;

using voe;

namespace voe{
    public class CardComponent : MonoBehaviour
    {
        [SerializeField]
        CardNameId card_id;
        [SerializeField]
        int player_owner = -1;

        [SerializeField] 
        SpriteRenderer card_owner_marker;
        [SerializeField] 
        SpriteRenderer card_image;
        
        public void set_card(CardNameId __card_id)
        {
            card_id = __card_id;
            card_image.sprite = Resources.Load<Sprite>(AssetDataBase.get_card_file_name(card_id));
            Assert.IsTrue(card_image.sprite);
            //Set card image
        }

        public void set_property(int _player_number){
            Assert.IsTrue(_player_number >= 0 && _player_number < 4);

            player_owner = _player_number+1;
            string name = "P"+player_owner.ToString();
            //Debug.Log(name);
            card_owner_marker.sprite = Resources.Load<Sprite>(name);
            Assert.IsTrue(card_owner_marker.sprite);
        }

        public CardNameId get_card_id()
        {
            return card_id;
        }
    }
}