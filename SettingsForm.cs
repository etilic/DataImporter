using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace DataImporter
{
    public partial class SettingsForm : Form
    {
        private Configuration config;

        public SettingsForm()
        {
            InitializeComponent();

            this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private void LoadConfig()
        {
            ConnectionStringsSection connectionStrings = 
                config.GetSection("connectionStrings") as ConnectionStringsSection;

            if (connectionStrings.ConnectionStrings["EtilicContext"] != null)
            {
                this.connectionStringTextBox.Text =
                    connectionStrings.ConnectionStrings["EtilicContext"].ConnectionString;
            }
        }

        private void SaveConfig()
        {
            ConnectionStringsSection connectionStrings =
                config.GetSection("connectionStrings") as ConnectionStringsSection;

            connectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("EtilicContext", this.connectionStringTextBox.Text, "System.Data.SqlClient"));

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("connectionStrings");
        }

        #region Cancel
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.SaveConfig();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override void OnShown(EventArgs e)
        {
            this.LoadConfig();

            base.OnShown(e);
        }
    }
}
