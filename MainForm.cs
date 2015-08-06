using CsvHelper;
using DataImporter.Names;
using Etilic.Core.DAL;
using Etilic.Core.Extensibility;
using Etilic.Name;
using Etilic.Name.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataImporter
{
    public partial class MainForm : Form
    {
        private ImportProgressForm progressForm;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Settings
        /// <summary>
        /// Opens a new instance of the <see cref="DataImporter.SettingsForm"/> class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(SettingsForm form = new SettingsForm())
            {
                form.ShowDialog();
            }
        }
        #endregion

        #region Import Name Data
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = null;

            using(OpenFileDialog dia = new OpenFileDialog())
            {
                dia.Filter = "CSV (*.csv)|*.csv|All files (*.*)|*.*";

                if (dia.ShowDialog() != DialogResult.OK)
                    return;

                filename = dia.FileName;
            }


            using(NameMapForm form = new NameMapForm())
            {
                using (TextReader r = new StreamReader(filename))
                {
                    CsvReader reader = new CsvReader(r);
                    reader.Configuration.HasHeaderRecord = true;
                    reader.Read();

                    for(Int32 i = 0; i < reader.FieldHeaders.Length; i++)
                    {
                        String str = reader.FieldHeaders[i];

                        NameColumn column = new NameColumn(i, str);

                        DataGridViewRow row = new DataGridViewRow();
                        DataGridViewTextBoxCell name = new DataGridViewTextBoxCell();
                        name.Value = column;

                        DataGridViewComboBoxCell type = new DataGridViewComboBoxCell();
                            
                        foreach(String property in Enum.GetNames(typeof(NameProperty)))
                        {
                            type.Items.Add(property);
                        }

                        row.Cells.Add(name);
                        row.Cells.Add(type);

                        form.DataGrid.Rows.Add(row);
                    }
                }

                if (form.ShowDialog() != DialogResult.OK)
                    return;

                NameImporter importer = new NameImporter(form.Culture);
                importer.DefaultYear = Convert.ToInt32(form.DefaultYear);

                foreach(DataGridViewRow row in form.DataGrid.Rows)
                {
                    NameProperty property = (NameProperty)Enum.Parse(
                        typeof(NameProperty), row.Cells[1].Value.ToString(), false);

                    importer.AddMapping(property, ((NameColumn)row.Cells[0].Value).Index);
                }

                using (progressForm = new ImportProgressForm())
                {
                    using(TextReader reader = new StreamReader(filename))
                    {
                        CsvReader csv = new CsvReader(reader);
                        CsvImportSource source = new CsvImportSource(csv);

                        importer.OnProcessing += delegate(Int64 entries, String name) {
                            progressForm.UpdateText(String.Format("Importing {0} ({1} entries processed)...", name, entries));
                        };
                        importer.OnCommitting += delegate(Int64 cached)
                        {
                            progressForm.UpdateText(String.Format("Committing {0} entries to the database...", cached));
                        };
                        progressForm.Worker.DoWork += new DoWorkEventHandler(delegate {
                            importer.Import(source);
                        });
                        progressForm.ShowDialog();
                    }
                }
            }
        }
        private void importSurnamesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void recalculateProbabilitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(ImportProgressForm form = new ImportProgressForm())
            {
                form.Worker.DoWork += RecalculateProbabilities;
                form.ShowDialog();
            }
        }

        void RecalculateProbabilities(object sender, DoWorkEventArgs e)
        {
            using(NameContext context = new NameContext())
            {
                List<Name> names = context.Names.ToList();

                foreach(Name name in names)
                {
                    List<NameUsage> usage = name.Usage.ToList();

                    foreach(NameUsage nu in usage)
                    {
                        nu.RecalculateProbability();
                    }
                }

                context.SaveChanges();
            }
        }

        private void sanitizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ImportProgressForm form = new ImportProgressForm())
            {
                form.Worker.DoWork += SanitizeNames;
                form.ShowDialog();
            }
        }

        private void SanitizeNames(object sender, DoWorkEventArgs e)
        {
            using (NameContext context = new NameContext())
            {
                List<Name> names = context.Names.ToList();

                foreach (Name name in names)
                {
                    name.Value = name.Value.Trim().ToUpper();
                }

                context.SaveChanges();
            }
        }

        
    }
}
