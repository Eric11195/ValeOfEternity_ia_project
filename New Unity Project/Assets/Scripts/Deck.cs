using System.Collections.Generic;
using System;
using System.Linq;

using voe;

public class Deck
{
    Stack<CardNameId> draw_pile;

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
    }

    public void put_card_on_top(CardNameId card_id)
    {
        draw_pile.Push(card_id);
    }
    public CardNameId draw()
    {
        return draw_pile.Pop();
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
    
}
