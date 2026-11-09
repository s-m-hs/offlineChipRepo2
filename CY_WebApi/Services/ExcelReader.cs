using CY_DM;
using ExcelDataReader;
namespace CY_WebApi.Services
{
    public class ExcelReader
    {
        public List<PartInfo>? ReadExcel(string filepath)
        {
            var partList = new List<PartInfo>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    // Assuming the data is in the first sheet
                    var table = result.Tables[0];

                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var partNumber = table.Rows[i][0].ToString();
                        var quantityText = table.Rows[i][1].ToString();
                        var manufacturerName = "";
                        if (table.Columns.Count > 2)
                        {
                            manufacturerName = table.Rows[i][2].ToString();
                        }

                        if (!string.IsNullOrEmpty(partNumber) && int.TryParse(quantityText, out int quantity))
                        {
                            partList.Add(new PartInfo
                            {
                                PartNumber = partNumber,
                                Quantity = quantity,
                                Manufacturer = string.IsNullOrEmpty(manufacturerName) ? null : manufacturerName,
                            });
                        }
                    }
                }
            }
            if (partList.Count > 0)
                return partList;
            return null;
        }

        public List<CyOrderItem>? ReadExcelV2(string filepath)
        {
            var partList = new List<CyOrderItem>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    // Assuming the data is in the first sheet
                    var table = result.Tables[0];

                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var partNumber = table.Rows[i][1].ToString();
                        var manufacturer = table.Rows[i][2].ToString();
                        var quantity = table.Rows[i][3].ToString();
                        var price = table.Rows[i][4].ToString();
                        var duration = table.Rows[i][5].ToString();
                        var description = table.Rows[i][6].ToString();

                        if (!string.IsNullOrEmpty(partNumber) && int.TryParse(quantity, out int quantityInt))
                        {
                            partList.Add(new CyOrderItem
                            {
                                PartNumber = partNumber,
                                Quantity = quantityInt,
                                Manufacturer = manufacturer,
                                Duration = duration,
                                Information = description,
                                StatusText = description,
                                UnitPrice = double.TryParse(price, out double priceDouble) ? priceDouble : 0,
                                CreateDate = DateTime.Now,
                                TotalPrice = priceDouble * quantityInt,
                                IsVisible = true
                            });
                        }
                    }
                }
            }
            if (partList.Count > 0)
                return partList;
            return null;
        }

        public List<PartInfo> ReadExcelForUpdating(string filepath)
        {
            var partList = new List<PartInfo>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    // Assuming the data is in the first sheet
                    var table = result.Tables[0];

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var partNumber = table.Rows[i][0].ToString();
                        var quantityText = table.Rows[i][1].ToString();

                        //For chipyab:
                        var manufac = "";

                        var priceString = "";
                        if (table.Columns.Count > 2)
                        {
                            //manufacturerName = table.Rows[i][2].ToString();
                            priceString = table.Rows[i][2].ToString();

                            //For chipyab
                            manufac = table.Rows[i][3].ToString();
                        }

                        if (!string.IsNullOrEmpty(partNumber) && int.TryParse(quantityText, out int quantity) && double.TryParse(priceString, out double price))
                        {
                            partList.Add(new PartInfo
                            {
                                PartNumber = partNumber,
                                Quantity = quantity,

                                //For chipyab:
                                Manufacturer = string.IsNullOrEmpty(manufac) ? null : manufac,
                                Price = price
                            });
                        }
                    }
                }
            }
            return partList;
        }
    }

    public class PartInfo
    {
        public string? PartNumber { get; set; }
        public string? Manufacturer { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}