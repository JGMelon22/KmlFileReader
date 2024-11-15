using System.Xml.Linq;
using SharpKml.Dom;
using SharpKml.Engine;

namespace HelloKmlSharp;

public class KmlFileReaderService
{
    private readonly string _kmlFile = Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, "DIRECIONADORES1" + Path.DirectorySeparatorChar, "DIRECIONADORES1.kml");

    public async Task ReadKmlFile(string cliente, string situacao, string bairro, string referencia, string ruaCruzamento)
    {
        KmlFile file;

        byte[] kmlBytes = await File.ReadAllBytesAsync(_kmlFile);

        using (MemoryStream stream = new MemoryStream(kmlBytes))
        {
            file = KmlFile.Load(stream);
        }


        foreach (Placemark placemark in file.Root.Flatten().OfType<Placemark>())
        {
            ExtendedData extendedData = placemark.ExtendedData;

            if (extendedData != null)
            {
                string clienteValue = GetDataValue(extendedData, "CLIENTE");
                string situacaoValue = GetDataValue(extendedData, "SITUAÇÃO");
                string bairroValue = GetDataValue(extendedData, "BAIRRO");
                string referenciaValue = GetDataValue(extendedData, "REFERENCIA");
                string ruaCruzamentoValue = GetDataValue(extendedData, "RUA/CRUZAMENTO");


                bool isClienteMatch = !string.IsNullOrEmpty(cliente) &&
                                      clienteValue != null &&
                                      clienteValue.Equals(cliente, StringComparison.OrdinalIgnoreCase);

                bool isSituacaoMatch = !string.IsNullOrEmpty(situacao) &&
                                       situacaoValue != null &&
                                       situacaoValue.Equals(situacao, StringComparison.OrdinalIgnoreCase);

                bool isBairroMatch = !string.IsNullOrEmpty(bairro) &&
                                     bairroValue != null &&
                                     bairroValue.Equals(bairro, StringComparison.OrdinalIgnoreCase);

                bool isReferenciaMatch = !string.IsNullOrEmpty(referencia) &&
                                         referenciaValue != null &&
                                         referenciaValue.Length >= 3 &&
                                         referenciaValue.Contains(referencia, StringComparison.OrdinalIgnoreCase);

                bool isRuaCruzamentoMatch = !string.IsNullOrEmpty(ruaCruzamento) &&
                                            ruaCruzamentoValue != null &&
                                            ruaCruzamentoValue.Length >= 3 &&
                                            ruaCruzamentoValue.Contains(ruaCruzamento, StringComparison.OrdinalIgnoreCase);


                if (isClienteMatch || isSituacaoMatch || isBairroMatch || isReferenciaMatch || isRuaCruzamentoMatch)
                {

                    XElement xmlElement = new XElement("ExtendedData");

                    foreach (Data data in extendedData.Data)
                    {
                        string sanitizedName = SanitizedXmlName(data.Name);
                        xmlElement.Add(new XElement(sanitizedName, data.Value));
                    }

                    XDocument formmatedXml = new XDocument(xmlElement);

                    Console.WriteLine(formmatedXml.ToString());
                }
            }
        }
    }

    private string GetDataValue(ExtendedData extendedData, string name)
        => extendedData.Data
            .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?.Value!;

    private string SanitizedXmlName(string name)
        => name.Replace('/', '_');
}
