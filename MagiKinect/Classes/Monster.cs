using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagiKinect.Classes
{
    class Monster : Card
    {
        // <nested class>
        // Properties: Extends a Tuple of type int, int. Used to store attack and defense of the card
        //             If they are picked up by the Kinect.
        // </nested class>
        public class Properties : Tuple<int, int>
        {
            public Properties(int Attack, int Defense)
                : base(Attack, Defense)
            {

            }

            public int Attack
            {
                get
                {
                    return this.Item1;
                }
            }

            public int Defense
            {
                get
                {
                    return this.Item2;
                }
            }
        }

        // <property>
        // _cardProperties: the properties of the card e.g. Attack, Defense.
        // </property>
        private Properties _cardProperties;
        public Properties CardProperties
        {
            get
            {
                return _cardProperties;
            }
        }

        public int Attack
        {
            get
            {
                return this._cardProperties.Attack;
            }
        }

        public int Defense
        {
            get
            {
                return this._cardProperties.Defense;
            }
        }
        
        public Monster(int id, string name, int _attack, int _defense) 
            : base(id, CardType.Creature, name, AssetHandler.DeckType.Unkown)
        {
            // construct properties.
            this._cardProperties = new Properties(_attack, _defense);
        }
    }
}
