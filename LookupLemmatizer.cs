using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using PluginContracts;
using OutputHelperLib;


namespace LookupLemmatizer
{
    public class LookupLemmatizer : Plugin
    {

        public string[] InputType { get; } = { "Tokens" };
        public string OutputType { get; } = "Tokens";

        public Dictionary<int, string> OutputHeaderData { get; set; } = new Dictionary<int, string>() { { 0, "TokenizedText" } };
        public bool InheritHeader { get; } = false;
        private Dictionary<string, string> LemmatizerDictionary { get; set; }
        private string SelectedLemmatizerLanguage { get; set; } = "English (en)";

        #region Plugin Details and Info

        public string PluginName { get; } = "Dictionary-based Lemmatizer";
        public string PluginType { get; } = "Lemmatizers";
        public string PluginVersion { get; } = "1.0.2";
        public string PluginAuthor { get; } = "Ryan L. Boyd (ryan@ryanboyd.io)";
        public string PluginDescription { get; } = "This plugin will lemmatize tokens that it receives from a tokenizer (e.g., the Twitter-Aware Tokenizer). Uses a linear \"lookup\" method, which means that this lemmatizer is dictionary-based. Dictionaries used for lemmatization are sourced from:" + Environment.NewLine +
            "https://github.com/michmech/lemmatization-lists";
        public bool TopLevel { get; } = false;
        public string PluginTutorial { get; } = "Coming Soon";

        public Icon GetPluginIcon
        {
            get
            {
                return Properties.Resources.icon;
            }
        }

        #endregion



        public void ChangeSettings()
        {

            using (var form = new SettingsForm_LookupLemmatizer(SelectedLemmatizerLanguage))
            {

                form.Icon = Properties.Resources.icon;
                form.Text = PluginName;

                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {

                    SelectedLemmatizerLanguage = form.SelectedModel;
                }
            }

        }





        public Payload RunPlugin(Payload Input)
        {

            Payload pData = new Payload();
            pData.FileID = Input.FileID;
            pData.SegmentID = Input.SegmentID;


            for (int counter = 0; counter < Input.StringArrayList.Count; counter++)
            {

                string[] TextToLemmatize = Input.StringArrayList[counter];
                //for this lemmatizer, we don't have to convert everything to lowercase if we don't want to.
                //to be determined if we'll actually do that

                for (int i = 0; i < TextToLemmatize.Length; i++)
                {
                    if (LemmatizerDictionary.ContainsKey(TextToLemmatize[i])) TextToLemmatize[i] = LemmatizerDictionary[TextToLemmatize[i]];
                }
                pData.StringArrayList.Add(TextToLemmatize);
                pData.SegmentNumber.Add(Input.SegmentNumber[counter]);

            }


            return (pData);






        }



        public void Initialize()
        {

            LemmatizerDictionary = new Dictionary<string, string>();

            string LemmaDic = "";
            switch (SelectedLemmatizerLanguage)
            {
                case "Asturian (ast)":
                    LemmaDic = Properties.Resources.manual_lemmatization_ast;
                    break;

                case "Bulgarian (bg)":
                    LemmaDic = Properties.Resources.manual_lemmatization_bg;
                    break;

                case "Catalan (ca)":
                    LemmaDic = Properties.Resources.manual_lemmatization_ca;
                    break;

                case "Czech (cs)":
                    LemmaDic = Properties.Resources.manual_lemmatization_cs;
                    break;

                case "English (en)":
                    LemmaDic = Properties.Resources.manual_lemmatization_en;
                    break;

                case "Estonian (et)":
                    LemmaDic = Properties.Resources.manual_lemmatization_et;
                    break;

                case "French (fr)":
                    LemmaDic = Properties.Resources.manual_lemmatization_fr;
                    break;

                case "Galician (gl)":
                    LemmaDic = Properties.Resources.manual_lemmatization_gl;
                    break;

                case "German (de)":
                    LemmaDic = Properties.Resources.manual_lemmatization_de;
                    break;

                case "Hungarian (hu)":
                    LemmaDic = Properties.Resources.manual_lemmatization_hu;
                    break;

                case "Irish (ga)":
                    LemmaDic = Properties.Resources.manual_lemmatization_ga;
                    break;

                case "Manx Gaelic (gv)":
                    LemmaDic = Properties.Resources.manual_lemmatization_gv;
                    break;

                case "Italian (it)":
                    LemmaDic = Properties.Resources.manual_lemmatization_it;
                    break;

                case "Persian/Farsi (fa)":
                    LemmaDic = Properties.Resources.manual_lemmatization_fa;
                    break;

                case "Portuguese (pt)":
                    LemmaDic = Properties.Resources.manual_lemmatization_pt;
                    break;

                case "Romanian (ro)":
                    LemmaDic = Properties.Resources.manual_lemmatization_ro;
                    break;

                case "Scottish Gaelic (gd)":
                    LemmaDic = Properties.Resources.manual_lemmatization_gd;
                    break;

                case "Slovak (sk)":
                    LemmaDic = Properties.Resources.manual_lemmatization_sk;
                    break;

                case "Slovene (sl)":
                    LemmaDic = Properties.Resources.manual_lemmatization_sl;
                    break;

                case "Spanish (es)":
                    LemmaDic = Properties.Resources.manual_lemmatization_es;
                    break;

                case "Swedish (sv)":
                    LemmaDic = Properties.Resources.manual_lemmatization_sv;
                    break;

                case "Ukrainian (uk)":
                    LemmaDic = Properties.Resources.manual_lemmatization_uk;
                    break;

                case "Welsh (cy)":
                    LemmaDic = Properties.Resources.manual_lemmatization_cy;
                    break;

                default:
                    LemmaDic = "";
                    break;
            }

            string[] LemmaLines = LemmaDic.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            string sep = "\t";

            for (int i = 0; i < LemmaLines.Length; i++)
            {
                string[] LemmaTokens = LemmaLines[i].Split(sep.ToCharArray());

                if ((LemmaTokens.Length == 2) && (!LemmatizerDictionary.ContainsKey(LemmaTokens[0])))
                {
                    LemmatizerDictionary.Add(LemmaTokens[0], LemmaTokens[1]);
                }

            }

        }

        public bool InspectSettings()
        {
            return true;
        }


        public Payload FinishUp(Payload Input)
        {
            return (Input);
        }




        #region Import/Export Settings
        public void ImportSettings(Dictionary<string, string> SettingsDict)
        {
            SelectedLemmatizerLanguage = SettingsDict["SelectedLemmatizerLanguage"];
        }

        public Dictionary<string, string> ExportSettings(bool suppressWarnings)
        {
            Dictionary<string, string> SettingsDict = new Dictionary<string, string>();
            SettingsDict.Add("SelectedLemmatizerLanguage", SelectedLemmatizerLanguage);
            return (SettingsDict);
        }
        #endregion

        



    }
}
