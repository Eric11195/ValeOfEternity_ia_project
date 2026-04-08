using UnityEngine;
using UnityEngine.Assertions;

using voe;

using System.Collections.Generic;
using System;
using System.Collections;
namespace voe{
    public class GameManager : MonoBehaviour
    {
        public int initial_number_of_players = 1;

        public Deck deck;

        public CardList market;

        public List<Player> players;

        [SerializeField]
        GameObject hand_area;
        [SerializeField]
        GameObject market_area;

        public void Init()
        {
            Debug.Log("Init Called");

            deck = new Deck();
            market = new CardList();
            players = new List<Player>();
            for(int i = 0; i < initial_number_of_players; ++i){
                players.Add(new Player());
            }
        }

        public void Start()
        {
            Init();
            StartCoroutine(market_round());
        }

        IEnumerator market_round()
        {
            Debug.Log("Market: CALL STARTED");
            foreach(Player p in players)
            {
                add_card_to_market();
                add_card_to_market();
            }
            CardList temporal_market = market.clone();
            for(int i = 0; i < players.Count; ++i)
            {
                Debug.Log("Market: Choose card started");
                Player p = players[i];
                p.current_card_pool_option = temporal_market;

                CoroutineWithData<CardNameId> cd = new CoroutineWithData<CardNameId>(this, p.choose_card_on_market() );
                yield return cd.coroutine;
                CardNameId cni = cd.result;
                
                yield return cni;

                temporal_market.extract(cni);
                p.add_chosen_at_market(cni);
                Debug.Log("Market: Choose card End");
            }
            for(int i = players.Count-1; i >= 0; --i)
            {
                Debug.Log("Market: Choose card started");
                Player p = players[i];
                p.current_card_pool_option = temporal_market;

                CoroutineWithData<CardNameId> cd = new CoroutineWithData<CardNameId>(this, p.choose_card_on_market() );
                yield return cd.coroutine;
                CardNameId cni = cd.result;
                
                yield return cni;

                temporal_market.extract(cni);
                p.add_chosen_at_market(cni);
                Debug.Log("Market: Choose card End");
            }
            Debug.Log("Market: CALL ENDED");
        }

        private void add_card_to_market()
        {
            Assert.IsTrue(deck.size() > 0);

            CardNameId card_id = deck.draw();

            market.add(card_id);
            market_area.GetComponent<CardAreaManager>().add(card_id);
        }
    }
}
