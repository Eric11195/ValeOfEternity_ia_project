using UnityEngine;

using System.Collections.Generic;
using System;

using voe;

namespace voe{
    public class CardList
    {
        List<CardNameId> card_list;

        public CardList()
        {
            card_list = new List<CardNameId>(0);
        }
        public CardList(ref List<CardNameId> list)
        {
            card_list = new List<CardNameId>(list);
        }

        public void add(CardNameId id)
        {
            card_list.Add(id);
        }

        public void extract(int idx)
        {
            card_list.RemoveAt(idx);
        }
        public int extract(CardNameId id)
        {
            int index = card_list.FindIndex(id => id.Equals(id) );
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
            return new CardList(ref card_list);
        }
    }
}