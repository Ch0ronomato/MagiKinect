using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;

namespace MagiKinect
{
    public struct MonsterSummonedDetail
    {
        public int attack, defense;
        public string name;
        public AssetHandler.DeckType type;
    }
    class KinectManager
    {
        KinectSensor Kinect;
        private RecognizerInfo _ri;
        public RecognizerInfo Recognizer
        {
            get
            {
                return _ri;
            }
        }

        private SpeechRecognitionEngine engine;
        public SpeechRecognitionEngine Engine
        {
            get
            {
                return engine;
            }
        }

        private static KinectManager _instance;
        public static KinectManager Instance
        {
            get
            {
                return _instance;
            }
        }
        // Event for land played
        public delegate void OnLandPlayed(object sender, AssetHandler.DeckType type);
        public event OnLandPlayed LandPlayed;

        // Event for monster summoned
        public delegate void OnMonsterSummoned(object sender, MonsterSummonedDetail detail);
        public event OnMonsterSummoned MonsterSummoned;

        // Event for end turn
        public delegate void OnEndTurn();
        public event OnEndTurn EndTurn;

        public KinectManager()
        {
            // Store static instance of the KinectManager.
            KinectManager._instance = this;
        }

        public void FindKinect()
        {
            bool foundKinect = false;
            foreach (var potentialKinect in KinectSensor.KinectSensors)
            {
                if (potentialKinect.Status == KinectStatus.Connected)
                {
                    Kinect = potentialKinect;
                    foundKinect = true;
                }
            }

            if (!foundKinect)
            {
                // throw new KinectNotFoundException();
            }

            // start the kinect!
            Kinect.Start();
            Kinect.ElevationAngle = 15;
        }

        public void SetupKinectRecognizer()
        {
            bool found = false;
            foreach (var recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    _ri = recognizer;
                    break;
                }
            }

            if (!found)
            {
                // throw new RecognizerNotFoundException();
            }

            // Recognizer found.
            this.engine = new SpeechRecognitionEngine(_ri.Id);
            this.engine.SpeechRecognized += SpeechRecognized;

            // Add definite grammer
            Choices choices = new Choices();
            choices.Add(new SemanticResultValue("I", "I"));
            choices.Add(new SemanticResultValue("Mountain", "MOUNTAIN"));
            choices.Add(new SemanticResultValue("Plains", "PLAINS"));
            choices.Add(new SemanticResultValue("Swamp", "SWAMP"));
            choices.Add(new SemanticResultValue("Forest", "FOREST"));
            choices.Add(new SemanticResultValue("Island", "ISLAND"));
            choices.Add(new SemanticResultValue("End my turn", "END_MY_TURN"));
            var gb = new GrammarBuilder { Culture = _ri.Culture };
            gb.Append(choices);
            var g = new Grammar(gb);

            // Setup summon wildcard.
            GrammarBuilder wildcardBuffer = new GrammarBuilder();
            wildcardBuffer.AppendWildcard();
            SemanticResultKey pwd = new SemanticResultKey("Summon", wildcardBuffer);
            GrammarBuilder test = new GrammarBuilder("I summon") + pwd;

            // Load grammer.
            this.engine.LoadGrammar(g);
            this.engine.LoadGrammar(new Grammar(test));
            try
            {
                engine.SetInputToAudioStream(Kinect.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.engine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception e)
            {

            }
        }

        public void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.3;
            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                switch (e.Result.Semantics.Value.ToString())
                {
                    case "FOREST":
                        {
                            this.LandPlayed.Invoke(this, AssetHandler.DeckType.Green);
                            break;
                        }
                    case "MOUNTAIN":
                        {
                            this.LandPlayed.Invoke(this, AssetHandler.DeckType.Red);
                            break;
                        }
                    case "ISLAND":
                        {
                            this.LandPlayed.Invoke(this, AssetHandler.DeckType.Blue);
                            break;
                        }
                    case "SWAMP":
                        {
                            this.LandPlayed.Invoke(this, AssetHandler.DeckType.Black);
                            break;
                        }
                    case "PLAINS":
                        {
                            this.LandPlayed.Invoke(this, AssetHandler.DeckType.White);
                            break;
                        }
                    case "I SUMMON":
                        {
                            MonsterSummonedDetail detail = this.GetDetailFromEvent(e.Result);
                            this.MonsterSummoned.Invoke(this, detail);
                            break;
                        }
                    case "END_MY_TURN":
                        {
                            this.EndTurn.Invoke();
                            break;
                        }
                }
            }
        }

        private MonsterSummonedDetail GetDetailFromEvent(RecognitionResult result)
        {
            MonsterSummonedDetail detail = new MonsterSummonedDetail();

            var q = result.ConstructSmlFromSemantics();
            System.IO.Stream writer = new System.IO.FileStream(@".\output.wav", System.IO.FileMode.Create);

            result.Audio.WriteToWaveStream(writer);
            writer.Close();
            // TODO: figure this out.
            return detail;
        }
    }
}
