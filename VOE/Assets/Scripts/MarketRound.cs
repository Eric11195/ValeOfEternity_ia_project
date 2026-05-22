using UnityEngine;
using UnityEngine.Assertions;

using System.Collections.Generic;
using System;
using System.Collections;

namespace voe
{
    public static class MarketRound
    {
        public static IEnumerator market_round()
        {
            GameManager gm = GameManager.get_instance();

            Logger.LogH2("Market phase started");
            List<GameObject> physical_card_list = new List<GameObject>(0);
            foreach (Player p in gm.players)
            {
                physical_card_list.Add(add_card_to_market());
                physical_card_list.Add(add_card_to_market());
            }
            CardList temporal_market = GameManager.get_instance().market.clone();
            for (int i = 0; i < gm.players.Count; ++i)
            {
                Player p = gm.get_player_with_idx_i_from_turn_player(i);
                p.current_card_pool_option = temporal_market;

                CoroutineWithData<CardNameId> cd = new CoroutineWithData<CardNameId>(gm, p.choose_card_on_market());
                yield return cd.coroutine;
                CardNameId cni = cd.result;

                yield return cni;

                int index = temporal_market.extract(cni);
                physical_card_list[index].GetComponent<CardComponent>().set_property(i);
                physical_card_list.RemoveAt(index);

                p.add_chosen_at_market(cni);
            }
            for (int i = gm.players.Count - 1; i >= 0; --i)
            {
                Player p = gm.get_player_with_idx_i_from_turn_player(i);
                p.current_card_pool_option = temporal_market;

                CoroutineWithData<CardNameId> cd = new CoroutineWithData<CardNameId>(gm, p.choose_card_on_market());
                yield return cd.coroutine;
                CardNameId cni = cd.result;

                yield return cni;

                int index = temporal_market.extract(cni);
                physical_card_list[index].GetComponent<CardComponent>().set_property(i);
                physical_card_list.RemoveAt(index);

                p.add_chosen_at_market(cni);
                //Debug.Log("Market: Choose card End");
            }
            //Debug.Log("Market: CALL ENDED");
            gm.market = new CardList();
        }

        private static GameObject add_card_to_market()
        {
            GameManager gm = GameManager.get_instance();

            Assert.IsTrue(gm.deck.size() > 0);

            CardNameId card_id = gm.deck.draw();

            gm.market.add(card_id);
            return gm.market_area.add(card_id);
        }
    }
}