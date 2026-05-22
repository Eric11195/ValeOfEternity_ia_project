using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using voe;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace voe{
    public class CardList
    {
        public List<CardNameId> card_list;

        public CardList get_deep_copy()
        {
            CardList cl = new();
            foreach(var cni in this.card_list)
            {
                cl.card_list.Add(cni);
            }
            return cl;
        }
        public CardList()
        {
            card_list = new List<CardNameId>(0);
        }
        public CardList(List<CardNameId> list)
        {
            card_list = new List<CardNameId>(list);
        }

        public bool contains(CardNameId id){
            return card_list.Contains(id);
        }
        public void add(CardNameId id)
        {
            card_list.Add(id);
        }

        public void extract(int idx)
        {
            card_list.RemoveAt(idx);
        }
        public CardNameId pop()
        {
            var card = card_list[0];
            card_list.RemoveAt(0);
            return card;
        }
        public CardNameId peek(){
            Assert.IsTrue(card_list.Count > 0);
            return card_list[0];
        }
        public bool empty()
        {
            return card_list.Count == 0;
        }
        public int extract(CardNameId id)
        {
            int index = card_list.FindIndex(x => x.Equals(id) );
            card_list.RemoveAt(index);
            return index;
        }
        public CardNameId get(int idx)
        {
            return card_list[idx];
        }

        public int size()
        {
            return card_list.Count;
        }

        public CardList clone()
        {
            return new CardList(card_list);
        }
        public int count_families()
        {
            int families = 0;
            var flags = CardFamily.None;
            foreach(var cni in card_list){
                var card = CardData.get_card(cni);
                if ((card.family & flags) == 0){
                    flags |= card.family;
                    ++families;
                }
            }
            return families;
        }
        public int count_card_of_family(CardFamily fam)
        {
            int cards = 0;
            foreach (var cni in card_list)
            {
                var card = CardData.get_card(cni);
                if ((card.family & fam) != 0)
                {
                    ++cards;
                }
            }
            return cards;
        }
        public int count_cards_with_clocks()
        {
            int cards = 0;
            foreach (var cni in card_list)
            {
                var card = CardData.get_card(cni);
                if (card.clockEffect != CardFuncs.void_func)
                {
                    ++cards;
                }
            }
            return cards;
        }
    }
}