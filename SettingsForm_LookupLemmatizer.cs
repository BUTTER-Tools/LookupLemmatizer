using System.IO;
using System.Windows.Forms;

namespace LookupLemmatizer
{
    internal partial class SettingsForm_LookupLemmatizer : Form
    {


        #region Get and Set Options

        public string SelectedModel { get; set; }

       #endregion



        public SettingsForm_LookupLemmatizer(string SelectedModel)
        {
            InitializeComponent();


            LemmatizerLanguageSelector.Items.AddRange(new string[] {
                                                                    "Asturian (ast)",
                                                                    "Bulgarian (bg)",
                                                                    "Catalan (ca)",
                                                                    "Czech (cs)",
                                                                    "English (en)",
                                                                    "Estonian (et)",
                                                                    "French (fr)",
                                                                    "Galician (gl)",
                                                                    "German (de)",
                                                                    "Hungarian (hu)",
                                                                    "Irish (ga)",
                                                                    "Manx Gaelic (gv)",
                                                                    "Italian (it)",
                                                                    "Persian/Farsi (fa)",
                                                                    "Portuguese (pt)",
                                                                    "Romanian (ro)",
                                                                    "Scottish Gaelic (gd)",
                                                                    "Slovak (sk)",
                                                                    "Slovene (sl)",
                                                                    "Spanish (es)",
                                                                    "Swedish (sv)",
                                                                    "Ukrainian (uk)",
                                                                    "Welsh (cy)"
                                                                    });

            
            try
            {
                LemmatizerLanguageSelector.SelectedIndex = LemmatizerLanguageSelector.FindStringExact(SelectedModel);
            }
            catch
            {
                LemmatizerLanguageSelector.SelectedIndex = 0;
            }


        }



       

        private void OKButton_Click(object sender, System.EventArgs e)
        {
            this.SelectedModel = LemmatizerLanguageSelector.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
        }
    }
}
