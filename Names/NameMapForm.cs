using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataImporter.Names
{
    public partial class NameMapForm : Form
    {
        public DataGridView DataGrid
        {
            get
            {
                return this.dataGridView1;
            }
        }

        public CultureInfo Culture
        {
            get
            {
                return this.comboBox1.SelectedItem as CultureInfo;
            }
        }

        public String DefaultYear
        {
            get
            {
                return this.yearTextBox.Text;
            }
        }

        public NameMapForm()
        {
            InitializeComponent();
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in this.dataGridView1.Rows)
            {
                DataGridViewComboBoxCell type = row.Cells[1] as DataGridViewComboBoxCell;

                if(type.Value == null)
                {
                    MessageBox.Show(String.Format("You need to select a mapping for {0}.", row.Cells[0].Value.ToString()));
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NameMapForm_Shown(object sender, EventArgs e)
        {
            foreach(var culture in CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(x => x.ToString()))
            {
                this.comboBox1.Items.Add(culture);
            }

            this.comboBox1.SelectedItem = CultureInfo.CurrentCulture;
        }
    }
}
