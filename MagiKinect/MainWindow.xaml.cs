using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using MagiKinect.Classes;

namespace MagiKinect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AssetHandler Assets;
        UIManager Manager;
        public static MainWindow Instance;
        public Canvas PlayerOneField
        {
            get
            {
                return FieldOne;
            }
        }
        public Canvas PlayerTwoField
        {
            get
            {
                return FieldTwo;
            }
        }

        public MainWindow()
        {
            // Store static instance of main window.
            MainWindow.Instance = this;

            // Initalize the component.
            InitializeComponent();

            // Setup assets.
            Assets = AssetHandler.SetupAssets();

            // Setup Animator
            Animator animator = new Animator();

            // Setup KinectManager
            KinectManager km = new KinectManager();

            // Attempt to find the Kinect.
            km.FindKinect();

            // Setup audio recognition.
            km.SetupKinectRecognizer();

            // Setup UIManager
            Manager = new UIManager(km);

            // Setup PlayerManager
            PlayerManager pm = new PlayerManager(0);

            // Start with two players.
            pm.StartupWithTwoPlayers();
        }
    }
}
