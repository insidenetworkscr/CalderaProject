using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TallerCaldera2.Models;

namespace TallerCaldera2.PdfDocuments
{
    public class ProformaPdf : IDocument
    {
        private readonly Proforma _proforma;

        public ProformaPdf(Proforma proforma)
        {
            _proforma = proforma;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Content().Column(col =>
                {
                    col.Item().Text($"PROFORMA {_proforma.Codigo}")
                        .FontSize(20).Bold();

                    col.Item().Text($"Cliente: {_proforma.ClienteNombre}");
                    col.Item().Text($"Email: {_proforma.ClienteEmail}");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.ConstantColumn(80);
                            c.ConstantColumn(100);
                            c.ConstantColumn(100);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Text("Descripción").Bold();
                            h.Cell().Text("Cant").Bold();
                            h.Cell().Text("Precio").Bold();
                            h.Cell().Text("Subtotal").Bold();
                        });

                        foreach (var i in _proforma.Items)
                        {
                            table.Cell().Text(i.Descripcion);
                            table.Cell().Text(i.Cantidad.ToString());
                            table.Cell().Text(i.PrecioUnitario.ToString("C"));
                            table.Cell().Text(i.Subtotal.ToString("C"));
                        }
                    });

                    col.Item().AlignRight().Text($"TOTAL: {_proforma.Total:C}")
                        .FontSize(16).Bold();
                });
            });
        }
    }
}
