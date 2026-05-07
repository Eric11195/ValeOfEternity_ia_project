using UnityEngine;
using UnityEngine.Assertions;

using voe;

using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.UI;

namespace voe{
    public class GameManager : MonoBehaviour
    {
        private bool just_changed_player = true;
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
        [SerializeField]
        List<GameObject> player_eyes;

        public GameObject stone_markers_parent;
        private int watching_player_idx = 0;

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
            watch_player_i(0);
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
            paint_player_hand(watching_player_idx);
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

        public void update_all_stones_representation(){
            int i = 0;
            Debug.Log("Update stone representation");
            foreach (Transform child in stone_markers_parent.transform)
            {
                update_stone_representation_from_player(players[i], child.gameObject);
                ++i;
                if(i >= initial_number_of_players) break;
                //child is your child transform
            }
        }
        private void update_stone_representation_from_player(Player p, GameObject parent){
            Debug.Log("Updating stone representation");
            parent.GetComponent<StoneRepresentator>().set_stones(p.stone_manager.sa);
        }
        private void paint_player_hand(Player p)
        {
            if (p.hand_representation_needs_update || just_changed_player)
            {
                hand_area.empty();
                foreach (CardNameId cni in p.hand.card_list)
                {
                    hand_area.add(cni);
                }

                p.hand_representation_needs_update = false;
                just_changed_player = false;
            }
        }
        private void paint_player_hand(int player_idx)
        {
            Assert.IsTrue(player_idx >= 0 && player_idx < players.Count);
            paint_player_hand(players[player_idx]);
        }
        public void watch_next_player()
        {
            watching_player_idx += 1;
            watching_player_idx %= players.Count;
            watch_player_i(watching_player_idx);
        }
        public void watch_player_i(int player_idx)
        {
            Assert.IsTrue(player_idx >= 0 && player_idx < players.Count);
            watching_player_idx = player_idx;
            just_changed_player = true;
            set_markers_of_viewing(watching_player_idx);
        }
        private void set_markers_of_viewing(int player_idx)
        {
            for(int i = 0; i < player_eyes.Count; ++i)
            {
                Sprite spr;
                if(player_idx == i) 
                    spr = Resources.Load<Sprite>("Marker_Eye");
                else spr = Resources.Load<Sprite>("Marker_"+(i+1));
                player_eyes[i].GetComponentInChildren<Image>().sprite = spr;
            }
        }
    }
}
