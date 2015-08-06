using CsvHelper;
using Etilic.Name.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Names
{
    /// <summary>
    /// Implements the <see cref="Etilic.Name.Import.INameImportSource"/> interface for the
    /// <see cref="CsvHelper.CsvReader"/> class.
    /// </summary>
    public class CsvImportSource : INameImportSource
    {
        #region Instance members
        /// <summary>
        /// The underlying <see cref="CsvHelper.CsvReader"/> object.
        /// </summary>
        private CsvReader reader;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialises a new instance of this class which is a wrapper around an 
        /// <see cref="CsvHelper.CsvReader"/> object that implements the 
        /// <see cref="Etilic.Name.Import.INameImportSource"/> interface.
        /// </summary>
        /// <param name="reader">
        /// The underlying <see cref="CsvHelper.CsvReader"/> object.
        /// </param>
        public CsvImportSource(CsvReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            this.reader = reader;
        }
        #endregion

        #region Read
        /// <summary>
        /// Reads the next row from the input, starting from the start of the file.
        /// </summary>
        /// <returns></returns>
        public Boolean Read()
        {
            return this.reader.Read();
        }
        #endregion

        #region Get
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public String Get(int index)
        {
            return this.reader.CurrentRecord[index];
        }
        #endregion
    }
}
