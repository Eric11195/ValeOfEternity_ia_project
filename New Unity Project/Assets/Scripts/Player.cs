using UnityEngine;

using voe;

using UnityEngine.Assertions;
using System.Collections;

namespace voe{
    public class Player
    {

        CardList hand;
        CardList table;
        CardList chosen_at_market;

        //This stores the card which it can currently select from
        public CardList current_card_pool_option;

        public Player()
        {
            hand = new CardList();
            table = new CardList();
            chosen_at_market = new CardList();
        }

        public IEnumerator choose_card_on_market()
        {
            Assert.IsTrue(current_card_pool_option.size() > 0);

            yield return current_card_pool_option.get(0);
        }

        public void add_chosen_at_market(CardNameId cni)
        {
            chosen_at_market.add(cni);
        }
    }
}