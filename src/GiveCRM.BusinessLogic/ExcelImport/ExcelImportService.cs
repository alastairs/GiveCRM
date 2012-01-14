using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GiveCRM.ImportExport;
using GiveCRM.Models;
using Ninject.Extensions.Logging;

namespace GiveCRM.BusinessLogic.ExcelImport
{
    public class ExcelImportService : IExcelImportService
    {
        private readonly IExcelImport importer;
        private readonly IMemberService memberService;
        private readonly IMemberFactory memberFactory;
        private readonly ILogger logger;

        public ExcelImportService(IExcelImport importer, IMemberService memberService, IMemberFactory memberFactory, ILogger logger)
        {
            if (importer == null)
            {
                throw new ArgumentNullException("importer");
            }

            if (memberService == null)
            {
                throw new ArgumentNullException("memberService");
            }

            if (memberFactory == null)
            {
                throw new ArgumentNullException("memberFactory");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.importer = importer;
            this.memberService = memberService;
            this.memberFactory = memberFactory;
            this.logger = logger;
        }

        public event Action<object, ImportDataCompletedEventArgs> ImportCompleted;
        public event Action<object, ImportDataFailedEventArgs> ImportFailed;

        public void Import(ExcelFileType fileType, Stream file)
        {
            //  FIELDS THAT CANNOT BE NULL!
            //    Reference
            //    FirstName
            //    LastName
            //    Salutation
            //    [Archived] - Not in template.

            try
            {
                importer.Open(file, fileType, hasHeaderRow: true);

                const int SheetIndex = 0; // Hard-coded for now
                IList<IDictionary<string, object>> rowsAsKeyValuePairs =
                    importer.GetRowsAsKeyValuePairs(SheetIndex).ToList();

                AddArchivedFieldToData(rowsAsKeyValuePairs); // This is a non-null field

                foreach (Member member in rowsAsKeyValuePairs.Select(memberData => memberFactory.CreateMember(memberData)))
                {
                    memberService.Insert(member);
                }

                InvokeImportDataCompleted();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred whilst importing from Excel");
                
                // The only exception that is explicitly thrown by the underlying code is an InvalidOperationException.
                // I would like to make this less catch-all, but there's probably a large number of exceptions that 
                // could be thrown by the import code, none of which are declared in the code itself.
                InvokeImportErrorFailed(ex);
            }
        }

        
        private static void AddArchivedFieldToData(IEnumerable<IDictionary<string, object>> rowsAsKeyValuePairs)
        {
            foreach (var row in rowsAsKeyValuePairs)
            {
                object archived;
                if (!row.TryGetValue("Archived", out archived))
                {
                    row["Archived"] = false;
                }
            }
        }


        private void InvokeImportDataCompleted()
        {
            var handler = ImportCompleted;
            if (handler != null)
            {
                handler(this, new ImportDataCompletedEventArgs());
            }
        }

        private void InvokeImportErrorFailed(Exception exception)
        {
            if (exception == null)
            {
                // No exception, no failure
                return;
            }

            var handler = ImportFailed;
            if (handler != null)
            {
                handler(this, new ImportDataFailedEventArgs(exception));
            }
        }
    }
}
