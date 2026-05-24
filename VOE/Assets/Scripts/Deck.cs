using System.Collections.Generic;
using System;
using System.Linq;

using voe;
using UnityEngine.Assertions;

namespace voe{
    public class Deck
    {
        Stack<CardNameId> draw_pile;
        public CardList discard_pile;

        public Deck()
        {
            List<CardNameId> aux = new List<CardNameId>();
            int number_of_cards = (int) CardNameId.NUMBER_OF_CARDS;
            for(int i = 0; i < number_of_cards; ++i)
            {
                aux.Add((CardNameId)i);
            }
            Shuffle(aux);
            draw_pile = new Stack<CardNameId>(aux);
            discard_pile = new CardList();
        }

        public void put_card_on_top(CardNameId card_id)
        {
            draw_pile.Push(card_id);
        }
        public CardNameId draw()
        {
            if (draw_pile.Count == 0) Reshuffle();
            Assert.IsTrue(GameManager.get_instance().deck.size() > 0);
            return draw_pile.Pop();
        }

        public int size()
        {
            return draw_pile.Count;
        }

        private static void Shuffle<T>(List<T> list)
        {
            Random random = new Random();
            int n = list.Count;

            // Start from the end and swap elements with a random one
            for (int i = n - 1; i > 0; i--) {
                int j = random.Next(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
        public void Reshuffle()
        {
            List<CardNameId> aux = new List<CardNameId>(0);
            while (!discard_pile.empty())
            {
                aux.Add(discard_pile.pop());
            }
            Shuffle(aux);
            draw_pile = new Stack<CardNameId>(aux);
            discard_pile = new CardList();
        }
        public void discard(CardNameId id)
        {
            discard_pile.add(id);
        }
        
    }
}