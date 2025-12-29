using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace HandHistories.Objects.Cards
{
    [DebuggerDisplay("{ToString()}")]
    [DataContract]
    public abstract class CardGroup : IEnumerable<Card>
    {
        [DataMember]
        protected readonly List<Card> Cards;

        protected CardGroup(params Card[] cards)
        {
            Cards = cards.ToList();

            if (Cards.Distinct().Count() != cards.Length)
            {
                throw new ArgumentException("Hole cards must be unique cards.");
            }
        }

        public static Card[] Parse(string cards)
        {
            if (cards == null)
            {
                return new Card[] {};
            }

            List<Card> cardsList = new List<Card>(2);
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] == ' ' || cards[i] == ',')
                {
                    continue;
                }
               cardsList.Add(new Card(cards[i++], cards[i]));
            }

            return cardsList.ToArray();
        }

        public void AddCard(Card card)
        {
            if (Cards.Contains(card))
            {
                throw new ArgumentException("Card " + card.ToString() + " already exists.");
            }

            if (Cards.Count >= GetMaxCardCount())
            {
                throw new ArgumentException("Cannot add more than " + GetMaxCardCount() + " cards.");
            }

            Cards.Add(card);
        }

        protected abstract int GetMaxCardCount();

        public void AddCards(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                AddCard(card);
            }
        }

        public List<Card> GetCards() { return Cards; }

        public Card this[int i]
        {
            get { return Cards[i]; }
        }

        public int[] GetIntValue()
        {
            return Cards.Select(c => c.CardIntValue).ToArray();
        }

        public int Count
        {
            get { return Cards.Count; }
        }

        public override string ToString()
        {
            return string.Join("", Cards.Select(c => c.ToString()));
        }

        public override bool Equals(object obj)
        {
            CardGroup cardGroup = obj as CardGroup;
            if (cardGroup == null) return false;

            return Cards.Count == cardGroup.Cards.Count && GetBitMask() == cardGroup.GetBitMask();
        }

        
        public static bool operator ==(CardGroup c1, CardGroup c2) 
        { 
            if (ReferenceEquals(c1, c2))
            {
                return true;
            }
            else if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
            {
                return false;
            }

            return c1.Equals(c2); 
        }
        

        public static bool operator !=(CardGroup c1, CardGroup c2) { return !(c1 == c2); }

        public override int GetHashCode()
        {
            return (Cards != null ? Cards.GetHashCode() : 0);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return Cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private ulong GetBitMask() 
        {
            return Cards.Aggregate(0ul, (mask, card) => mask | 1ul << card.CardIntValue);
        }
    }
}