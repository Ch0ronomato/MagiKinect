using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace MagiKinect.Classes
{
    class PlayerManager
    {
        // <property>
        // player(one, two): Booleans indicating which player is active
        // <property>
        private bool _playerOne, _playerTwo;
        public bool PlayerOne
        {
            set
            {
                _playerOne = value;
                _playerTwo = !value;
            }

            get
            {
                return _playerOne;
            }
        }

        public bool PlayerTwo
        {
            set
            {
                _playerTwo = value;
                _playerOne = !value;
            }

            get
            {
                return _playerTwo;
            }
        }

        // <property> Player
        // A enum indiciator the player
        // </property>
        public enum PlayerEnum
        {
            PlayerOne = 0,
            PlayerTwo = 1
        }

        // <property>
        // _players: A list of players active
        // </property>
        List<Player> _players = new List<Player>();
        public List<Player> Players
        {
            get
            {
                return _players;
            }
        }

        // <property>
        // _id: The id of the manager
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
        // _current: The current player 
        // </property>
        private PlayerEnum _current;
        public PlayerEnum CurrentPlayer
        {
            get
            {
                if (this.PlayerOne)
                {
                    return PlayerEnum.PlayerOne;
                }

                else
                {
                    return PlayerEnum.PlayerTwo;
                }
            }
        }

        private static PlayerManager _instance;
        public static PlayerManager Instance
        {
            get
            {
                return _instance;
            }
        }
        public PlayerManager(int id)
        {
            _instance = this;
            _id = id;
           
            // Bind to the land played event. 
            // This event will indicate to us that we are in a main phase.
            KinectManager.Instance.LandPlayed += OnLandPlayed;

            // Bind to the end turn event.
            // This event will indicate to switch players because a turn has ended.
            KinectManager.Instance.EndTurn += OnEndTurn;

            // Bind to the MonsterSummoned event.
            // This event will indicate that we a new monster has been summoned.
            KinectManager.Instance.MonsterSummoned += OnMonsterSummoned;
        }

        public void OnEndTurn()
        {
            // The animation has completed. Change the player.
            this.SwitchPlayers();

            // Add one card to the players hand
            this._players[(int)this.GetPlayer()].AddCardToHand();
        }

        public void OnMonsterSummoned(object sender, MonsterSummonedDetail detail)
        {
            // Get current player object.
            var player = this._players[(int)this.GetPlayer()];
            
            // Add to current players battlefield.
            player.AddMonsterToBattleField(detail.name, detail.attack, detail.defense);
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public void StartupWithTwoPlayers()
        {
            Player pOne = new Player(1);
            Player pTwo = new Player(2);

            this._players.Add(pOne);
            this._players.Add(pTwo);

            this._playerOne = true;
            this._current = PlayerEnum.PlayerOne;
        }

        public void OnLandPlayed(object sender, AssetHandler.DeckType color)
        {
            // Add land to player
            this._players[(int)this.GetPlayer()].AddLand(color);
        }

        private void SwitchPlayers()
        {
            // Invert the current player setup.
            PlayerOne = !PlayerOne;
        }

        private PlayerEnum GetPlayer()
        {
            if (PlayerOne) { return PlayerEnum.PlayerOne; }
            else { return PlayerEnum.PlayerTwo; }
        }
    }
}
