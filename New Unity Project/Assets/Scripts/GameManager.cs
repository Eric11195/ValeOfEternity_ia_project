using UnityEngine;
using UnityEngine.Assertions;

using voe;

using System.Collections.Generic;
using System;
using System.Collections;

namespace voe{
    public class GameManager : MonoBehaviour
    {
        public static GameManager _instance = null;

        public int initial_number_of_players = 1;

        public Deck deck;

        public CardList market;

        public List<Player> players;

        [SerializeField]
        public CardAreaManager hand_area;
        [SerializeField]
        public CardAreaManager market_area;
        [SerializeField]
        public CardAreaManager highlight_card_area;

        public static GameManager get_instance()
        {
            Assert.IsTrue(_instance != null, "You are calling this function before Init was called");
            return _instance;
        }

        public void Init()
        {
            Assert.IsTrue(_instance==null, "There was already a GameManager instance");
            _instance = this;

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
            StartCoroutine(game_loop());
            //StartCoroutine(market_round());
        }

        IEnumerator game_loop()
        {
            int round = 0;
            while (round <= 10 && !any_player_past_threshold())
            {
                ++round;
                Debug.Log("Round "+round+'\n');
                yield return StartCoroutine(MarketRound.market_round());
                yield return StartCoroutine(PlayCardsRound.play_cards_round());
                yield return StartCoroutine(ClockRound.clock_round());
            }
            yield return null;
        }

        public void Update()
        {
            highlight_card();
        }

        private void highlight_card()
        {
            Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouse_world_pos);
            //Debug.Log(hit);
            highlight_card_area.empty();
            if (hit != null)
            {
                GameObject item = hit.gameObject;
                Debug.Log(item);
                CardComponent cc = item.GetComponent<CardComponent>();
                if(!cc)return;

                CardNameId cni = cc.get_card_id();
                Debug.Log(cni);
                highlight_card_area.add(cni);
            }
        }

        private bool any_player_past_threshold()
        {
            int i = 0;
            while (i < players.Count && !players[i].points_past_threshold()) ++i;
            return i < players.Count;
        }
    }
}
