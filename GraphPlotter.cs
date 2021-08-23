using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;

namespace ilspy
{
    public class GraphPlotter
    {
        private readonly DataSet _dataSet;

        public GraphPlotter(DataSet dataSet)
        {
            this._dataSet = dataSet;
        }

        public void WriteToFile(String filename)
        {
            using (var streamWriter = new StreamWriter("data.json"))
            {
                string jsonString = JsonSerializer.Serialize(this._dataSet);
                streamWriter.WriteLine(jsonString);
            }

            // var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            // using (var reader = embeddedProvider.GetFileInfo("Resources/ChartTemplate.html").CreateReadStream())
            // {
            //     String htmlTemplate = null;
            //     using (var foo = new StreamReader(reader))
            //     {
            //         htmlTemplate = foo.ReadToEnd();
            //     }
            //     string jsonString = JsonSerializer.Serialize(this._dataSet);
            //     var renderedHtml = htmlTemplate.Replace("DATA_TEMPLATE", jsonString);
            //
            //     using (var streamWriter = new StreamWriter(filename))
            //     {
            //         streamWriter.WriteLine(renderedHtml);
            //     }
            // }
        }
    }
}