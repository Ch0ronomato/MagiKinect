using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagiKinect.Classes
{
    public class AnonymousCard { }
    class Card
    {
        // <param>
        // Id: The id the card (this is just a trivial id)
        // </param>
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
        }

        // <param>
        // Name: The name of the card. This property may or may not be filled, depending on the speech recognition.
        // </param>
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        // <param>
        // Color: The color of the card.
        // </param>
        private AssetHandler.DeckType _color;
        public AssetHandler.DeckType Color
        {
            get
            {
                return _color;
            }
        }

        // <property>
        // Type: The type of the card (Creature, Instant, Sorcery, Land etc)
        // <property>
        public enum CardType
        {
            Land = 0x000,
            Creature = 0x001,
            Instance = 0x002,
            Sorcery = 0x003,
            Unknown = 0x100
        }
        private CardType _type;
        public CardType Type
        {
            get
            {
                return _type;
            }
        }
        public Card(int id, CardType type = CardType.Unknown, string name = "", AssetHandler.DeckType color = AssetHandler.DeckType.Unkown)
        {
            this._id = id;
            this._name = name;
            this._type = type;
            this._color = color;
        }
    }
}
