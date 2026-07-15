using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using AdventureWorksVentas.DTOs;

namespace AdventureWorksVentas.Services;

public class VentaPdfService
{
    // Arma el PDF del comprobante de una venta: cabecera (cliente,
    // vendedor, fecha, territorio) + tabla de items + totales.
    // Si la venta esta anulada, se estampa un sello "ANULADA" bien
    // visible para que no se confunda con un comprobante valido.
    public byte[] Generar(VentaDTO venta, List<VentaDetalleDTO> detalle)
    {
        var documento = Document.Create(container =>
        {
            container.Page(pagina =>
            {
                pagina.Size(PageSizes.A4);
                pagina.Margin(30);
                pagina.DefaultTextStyle(x => x.FontSize(11));

                pagina.Header().Column(col =>
                {
                    col.Item().Text("AdventureWorksVentas").FontSize(20).Bold();
                    col.Item().Text($"Comprobante de Venta N.° {venta.SalesOrderID}").FontSize(14);

                    if (venta.Anulado)
                    {
                        col.Item().PaddingTop(5).Text("*** VENTA ANULADA ***")
                            .FontSize(16).Bold().FontColor(Colors.Red.Medium);
                    }

                    col.Item().PaddingTop(10).LineHorizontal(1);
                });

                pagina.Content().PaddingTop(15).Column(col =>
                {
                    col.Item().Text($"Fecha: {venta.OrderDate:yyyy-MM-dd}");
                    col.Item().Text($"Cliente: {venta.Cliente}");
                    col.Item().Text($"Vendedor: {venta.Vendedor}");
                    col.Item().Text($"Territorio: {venta.Territorio}");

                    if (venta.Anulado && venta.FechaAnulacion.HasValue)
                    {
                        col.Item().PaddingTop(5)
                            .Text($"Anulada el: {venta.FechaAnulacion:yyyy-MM-dd HH:mm}")
                            .FontColor(Colors.Red.Medium);
                    }

                    col.Item().PaddingTop(15).Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columnas =>
                        {
                            columnas.RelativeColumn(4);
                            columnas.RelativeColumn(1);
                            columnas.RelativeColumn(2);
                            columnas.RelativeColumn(1);
                            columnas.RelativeColumn(2);
                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Element(CeldaEncabezado).Text("Producto");
                            header.Cell().Element(CeldaEncabezado).Text("Cant.");
                            header.Cell().Element(CeldaEncabezado).Text("Precio");
                            header.Cell().Element(CeldaEncabezado).Text("Desc.");
                            header.Cell().Element(CeldaEncabezado).Text("Total");
                        });

                        foreach (var item in detalle)
                        {
                            var total = item.OrderQty * item.UnitPrice * (1 - item.UnitPriceDiscount);

                            tabla.Cell().Element(CeldaDato).Text(item.Producto);
                            tabla.Cell().Element(CeldaDato).Text(item.OrderQty.ToString());
                            tabla.Cell().Element(CeldaDato).Text(item.UnitPrice.ToString("C"));
                            tabla.Cell().Element(CeldaDato).Text(item.UnitPriceDiscount.ToString("P0"));
                            tabla.Cell().Element(CeldaDato).Text(total.ToString("C"));
                        }
                    });

                    col.Item().PaddingTop(15).AlignRight().Column(totales =>
                    {
                        totales.Item().Text($"Subtotal: {venta.SubTotal:C}");
                        totales.Item().Text($"IVA: {venta.TaxAmt:C}");
                        totales.Item().Text($"Flete: {venta.Freight:C}");
                        totales.Item().Text($"Total: {venta.TotalDue:C}").FontSize(13).Bold();
                    });
                });

                pagina.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Generado el ").FontSize(9);
                    x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).FontSize(9);
                });
            });
        });

        return documento.GeneratePdf();
    }

    private static IContainer CeldaEncabezado(IContainer contenedor) =>
        contenedor.DefaultTextStyle(x => x.Bold()).Padding(5).Background(Colors.Grey.Lighten2);

    private static IContainer CeldaDato(IContainer contenedor) =>
        contenedor.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
}