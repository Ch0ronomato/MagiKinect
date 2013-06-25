using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace MagiKinect.Classes
{
    class UIManager
    {
        // <KinectManager> kinectManager
        // Will handle all interactions with the kinect. Including audio recognition
        // </KinectManager>
        private KinectManager _kinectManager;
        public KinectManager KinectManager
        {
            get
            {
                return _kinectManager;
            }
        }

        public UIManager(KinectManager kinectManager)
        {
            // Store the kinect manager.
            _kinectManager = kinectManager;

            // Trap on land played
            _kinectManager.LandPlayed += OnLandPlayed;   
            
            // Bind to monster summoned.
            _kinectManager.MonsterSummoned += OnMonsterSummoned;
        }

        public void OnLandPlayed(object sender, AssetHandler.DeckType color)
        {
            // Determine the current player.
            PlayerManager.PlayerEnum current = PlayerManager.Instance.CurrentPlayer;
            
            // Set the backgrounds provided they are not already set
            var canvas = (Canvas)null;
            if (current == PlayerManager.PlayerEnum.PlayerOne)
            {
                canvas = MainWindow.Instance.FieldOne;
            }
            else
            {
                canvas = MainWindow.Instance.FieldTwo;
            }

            if (canvas.Background != AssetHandler.Instance.GetImageFromColor(color))
            {
                Animator.Instance.AnimateBackground(canvas, color);
            }
        }

        public void OnMonsterSummoned(object sender, MonsterSummonedDetail detail)
        {
            // Get current player.
            PlayerManager.PlayerEnum current = PlayerManager.Instance.CurrentPlayer;

            // Get canvas for player.
            var canvas = (Canvas)null;
            if (current == PlayerManager.PlayerEnum.PlayerOne)
            {
                canvas = MainWindow.Instance.FieldOne;
            }

            else
            {
                canvas = MainWindow.Instance.FieldTwo;
            }

            // Add card to field.
        }
    }
}
