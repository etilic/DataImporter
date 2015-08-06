using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataImporter
{
    public delegate void UpdateTextDelegate();

    public partial class ImportProgressForm : Form
    {
        private BackgroundWorker worker = new BackgroundWorker();

        public BackgroundWorker Worker
        {
            get { return this.worker; }
        }

        public ImportProgressForm()
        {
            InitializeComponent();

            this.worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done!", "Data Import", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            worker.RunWorkerAsync();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void UpdateText(String text)
        {
            UpdateTextDelegate fun = new UpdateTextDelegate(delegate
            {
                this.importLabel.Text = text;
            });

            if (this.InvokeRequired)
            {
                this.Invoke(fun);
            }
            else
            {
                fun();
            }
        }
    }
}
