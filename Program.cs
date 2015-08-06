using Etilic.Core.DAL;
using Etilic.Core.Extensibility;
using Etilic.Identity;
using Etilic.Name;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataImporter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Database.SetInitializer<EtilicContext>(null);

            BundleManager.RegisterBundle(new NameBundle());
            BundleManager.RegisterBundle(new IdentityBundle());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
