using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Helper
{
    public class CsvResult : FileResult
    {
        private readonly string _data;
        private readonly string _fileDownloadName;

        public CsvResult(string data, string fileDownloadName) : base("text/csv")
        {
            this._data = data;
            this._fileDownloadName = fileDownloadName;
        }
        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + _fileDownloadName });

            using (var streamWriter = new StreamWriter(response.Body))
            {
                await streamWriter.WriteAsync(_data);
                await streamWriter.FlushAsync();
            }
        }
    }
}
