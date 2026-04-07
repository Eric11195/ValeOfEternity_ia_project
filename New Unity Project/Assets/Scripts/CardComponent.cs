using UnityEngine;

using voe;

public class CardComponent : MonoBehaviour
{
    [SerializeField]
    CardNameId card_id;
    void set_card(CardNameId card_id)
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>(AssetDataBase.get_card_file_name(card_id));
        //Set card image
    }
}
