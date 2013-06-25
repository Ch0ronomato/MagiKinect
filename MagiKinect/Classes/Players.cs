using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagiKinect.Classes
{
    class Player
    {
        // <property>
        // _id: The id of the current player 
        // </property>
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
        }
        // <property>
        // Hand: A List of type Card that holds all the cards in the players hand.
        // </property>
        List<AnonymousCard> Hand = new List<AnonymousCard>(7);

        // <property>
        // Graveyard: A list of type Card that holds all the cards sent to the graveyard.
        // </property>
        List<Card> Graveyard = new List<Card>();

        // <property>
        // BattleGround: A list of type Card that holds all the cards the player has on the battle ground
        // </property>
        List<Card> BattleGround = new List<Card>();

        // <property>
        // Lands: A list of type Card that holds all the lands that the player holds.
        // </property>
        List<Card> Lands = new List<Card>();

        // <property>
        // _deckColor: The color of the players deck.
        // </property>
        AssetHandler.DeckType _deckColor;
        public AssetHandler.DeckType DeckColor
        {
            get
            {
                return _deckColor;
            }
        }

        public Player(int id = 0, AssetHandler.DeckType deckType = AssetHandler.DeckType.Unkown)
        {
            _id = id;
            _deckColor = deckType;  
        }

        public void DetermineDeckColor()
        {
            // iterate through hand, deck, etc, to try to determine deck.
            Dictionary<AssetHandler.DeckType, int> deckColorCount = new Dictionary<AssetHandler.DeckType, int>();

            // defaults
            {
                deckColorCount[AssetHandler.DeckType.Black] = 0; deckColorCount[AssetHandler.DeckType.Blue] = 0;
                deckColorCount[AssetHandler.DeckType.Green] = 0; deckColorCount[AssetHandler.DeckType.Red] = 0;
                deckColorCount[AssetHandler.DeckType.Unkown] = 0; deckColorCount[AssetHandler.DeckType.White] = 0;
            }

            foreach (Card card in BattleGround)
            {
                deckColorCount[card.Color]++;
            }

            deckColorCount.Max();
        }

        public void AddLand(AssetHandler.DeckType landColor, string name = "")
        {
            // construct land from enum.
            Card newLand = new Card(this.Lands.Count, Card.CardType.Land, name, landColor);

            this.Lands.Add(newLand);
        }

        public void AddCardToHand()
        {
            this.Hand.Add(new AnonymousCard());
        }

        public void AddMonsterToBattleField(string name, int attack, int defense)
        {
            int lastId = 0;
            if (this.BattleGround.Count > 0)
            {
                lastId = this.BattleGround.Last().Id;
            }

            this.BattleGround.Add(new Monster(lastId + 1, name, attack, defense));
        }
    }
}
