using System.Globalization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TallerCaldera2.Models;

namespace TallerCaldera2.PdfDocuments
{
    public class ProformaPdf : IDocument
    {
        private readonly Proforma _proforma;
        private readonly IWebHostEnvironment _env;
        private readonly CultureInfo _cr = new("es-CR"); // ₡ Colones

        public ProformaPdf(Proforma proforma, IWebHostEnvironment env)
        {
            _proforma = proforma;
            _env = env;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Content().Column(col =>
                {
                    // ===============================
                    // HEADER CON LOGO
                    // ===============================
                    col.Item().Background("#f1f5f9").Padding(15).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("PROFORMA")
                                .FontSize(20)
                                .Bold()
                                .FontColor("#1e3a8a");

                            c.Item().Text(_proforma.Codigo)
                                .FontSize(12)
                                .Bold();
                        });

                        row.ConstantItem(180).Column(c =>
                        {
                            var logoPath = Path.Combine(
                                _env.WebRootPath,
                                "images",
                                "Logo_Caldera.png"
                            );

                            if (File.Exists(logoPath))
                            {
                                c.Item()
                                 .AlignRight()
                                 .Height(60)
                                 .Image(logoPath, ImageScaling.FitArea);
                            }

                            c.Item().AlignRight()
                                .Text($"Fecha: {_proforma.FechaEmision:dd/MM/yyyy}");

                            c.Item().AlignRight()
                                .Text($"Válida hasta: {_proforma.FechaValidez:dd/MM/yyyy}");
                        });
                    });

                    col.Item().PaddingVertical(10);

                    // ===============================
                    // INFO CLIENTE / VEHÍCULO
                    // ===============================
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Cliente").Bold();
                            c.Item().Text(_proforma.ClienteNombre);
                            c.Item().Text(_proforma.ClienteEmail);
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Vehículo").Bold();
                            c.Item().Text($"{_proforma.Marca} {_proforma.Modelo}");
                        });
                    });

                    col.Item().PaddingVertical(15).LineHorizontal(1);

                    // ===============================
                    // TABLA ITEMS
                    // ===============================
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.ConstantColumn(60);
                            c.ConstantColumn(80);
                            c.ConstantColumn(90);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Background("#e5e7eb").Padding(5).Text("Descripción").Bold();
                            h.Cell().Background("#e5e7eb").Padding(5).AlignCenter().Text("Cant").Bold();
                            h.Cell().Background("#e5e7eb").Padding(5).AlignRight().Text("Precio").Bold();
                            h.Cell().Background("#e5e7eb").Padding(5).AlignRight().Text("Subtotal").Bold();
                        });

                        foreach (var i in _proforma.Items)
                        {
                            table.Cell().Padding(5).Text(i.Descripcion);
                            table.Cell().Padding(5).AlignCenter().Text(i.Cantidad.ToString());
                            table.Cell().Padding(5).AlignRight()
                                .Text(i.PrecioUnitario.ToString("C", _cr));
                            table.Cell().Padding(5).AlignRight()
                                .Text(i.Subtotal.ToString("C", _cr));
                        }
                    });

                    col.Item().PaddingVertical(15);

                    // ===============================
                    // TOTALES
                    // ===============================
                    col.Item().AlignRight().Column(c =>
                    {
                        c.Item().Text($"Subtotal: {_proforma.Subtotal.ToString("C", _cr)}");

                        if (_proforma.AplicarIva)
                        {
                            c.Item().Text($"IVA (13%): {_proforma.Iva.ToString("C", _cr)}");
                        }

                        c.Item()
                            .Background("#dcfce7")
                            .Padding(10)
                            .Text($"{(_proforma.AplicarIva ? "TOTAL CON IVA" : "TOTAL")}: {_proforma.TotalConIva.ToString("C", _cr)}")
                            .FontSize(15)
                            .Bold();
                    });

                    // ===============================
                    // IMÁGENES
                    // ===============================
                    if (_proforma.Images != null && _proforma.Images.Any())
                    {
                        col.Item().PaddingTop(20);

                        col.Item().Text("Evidencia fotográfica")
                            .FontSize(14)
                            .Bold();

                        col.Item().PaddingVertical(10);

                        col.Item().Row(row =>
                        {
                            foreach (var img in _proforma.Images)
                            {
                                var imagePath = Path.Combine(
                                    _env.WebRootPath,
                                    img.ImageUrl.TrimStart('/')
                                );

                                if (File.Exists(imagePath))
                                {
                                    row.RelativeItem()
                                       .Padding(5)
                                       .Height(130)
                                       .Background("#f8fafc")
                                       .Image(imagePath, ImageScaling.FitArea);
                                }
                            }
                        });
                    }
                });
            });
        }
    }
}