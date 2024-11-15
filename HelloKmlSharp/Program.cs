using HelloKmlSharp;

KmlFileReaderService kmlFileReaderService = new KmlFileReaderService();

await kmlFileReaderService.ReadKmlFile("DISCAR", "FALTA REINSTALAR", "CENTRO", "PRAÇA FAUSTO CARDOSO", "AV. IVO DO PRADO X TRAVESSA BENJAMIN CONSTANT");
