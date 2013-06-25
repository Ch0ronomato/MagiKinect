using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MagiKinect
{
    public struct AssetHandler
    {
        public ImageBrush Island, Mountain, Swamp, Forest, Plains;
        private Dictionary<DeckType, ImageBrush> TypeToImage;
        public static AssetHandler Instance;
        public enum DeckType
        {
            White=0x000,
            Red=0x001,
            Blue=0x002,
            Green=0x003,
            Black=0x004,
            Unkown=0x005,
        }

        public static AssetHandler SetupAssets()
        {
            AssetHandler handler = new AssetHandler();
            handler.TypeToImage = new Dictionary<DeckType, ImageBrush>();
            AssetHandler.Instance = handler;

            handler.Island = new ImageBrush(new BitmapImage(new Uri(@"..\..\Assets\Island.jpg", UriKind.Relative)));
            handler.Mountain = new ImageBrush(new BitmapImage(new Uri(@"..\..\Assets\Mountain.jpg", UriKind.Relative)));
            handler.Swamp = new ImageBrush(new BitmapImage(new Uri(@"..\..\Assets\Swamp.jpg", UriKind.Relative)));
            handler.Forest = new ImageBrush(new BitmapImage(new Uri(@"..\..\Assets\Forest.jpg", UriKind.Relative)));
            handler.Plains = new ImageBrush(new BitmapImage(new Uri(@"..\..\Assets\Plain.jpg", UriKind.Relative)));
            
            handler.TypeToImage[DeckType.White] = handler.Plains;
            handler.TypeToImage[DeckType.Red] = handler.Mountain;
            handler.TypeToImage[DeckType.Blue] = handler.Island;
            handler.TypeToImage[DeckType.Green] = handler.Forest;
            handler.TypeToImage[DeckType.Black] = handler.Swamp;
            return handler;
        }

        public ImageBrush GetImageFromColor(DeckType type)
        {
            return TypeToImage[type];
        }
    }
}
