

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using RedeNeural.DataTransferObjects;

namespace RedeNeural.Controllers
{
    public class FileReaderController
    {
        // private readonly List<FutebolDTO> data = new();

        internal static List<FutebolDTO> GetContent()
        {
            using var reader = new StreamReader("files/Futebol.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                Delimiter = ";"
            };

            var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<FutebolDTO>().ToList();
            Console.WriteLine(records);

            return records;
        }
    }
}