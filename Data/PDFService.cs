
using Microsoft.Maui.Controls;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Text.Json;
using static bislerium.Components.Pages.Reports;

namespace bislerium.Data
{
    public class PDFService
    {
        public static void SaveAllOrdersPDF(List<TopItem> topItems, float totalRevenue, List<Orders> orders, string fileName)
        {
            var appPath = Utils.GetAppDirectoryPath();
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(13));
                    page.Header().Text("Bislerium Cafe - Orders Till Date").Bold().FontSize(25);

                    page.Content().Column(column =>
                    {
                        column.Item().Text("Top 5s").SemiBold().FontSize(18);
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(column =>
                            {
                                column.RelativeColumn();
                                column.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Item Name").SemiBold();
                                header.Cell().Text("Revenue").SemiBold();
                            });

                            foreach (var item in topItems)
                            {
                                table.Cell().Text(item.ItemName);
                                table.Cell().Text("Rs. " + item.TotalRevenue);
                            }
                        });

                        column.Item().Text("All Transactions").SemiBold().FontSize(18);
                        column.Item().Table(table =>
                        {

                            table.ColumnsDefinition(column =>
                            {
                                column.RelativeColumn();
                                column.RelativeColumn();
                            });
                            table.Header(header =>
                            {
                                table.Cell().Text("Total Revenue").SemiBold();
                                table.Cell().Text("Rs. " + totalRevenue.ToString());
                            });
                        });

                        column.Item().Text("Orders").SemiBold().FontSize(18);
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Member Number").SemiBold();
                                header.Cell().Text("Items").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                                header.Cell().Text("Discount").SemiBold();
                            });

                            foreach (var order in orders)
                            {
                                table.Cell().Text(order.MemberNumber).ToString();
                                var menuItemsText = string.Join(", ", order.Items.Select(menuItem => MenuItemService.GetItemByID(menuItem.ItemID)?.Name ?? "Unknown"));
                                table.Cell().Text(menuItemsText);
                                table.Cell().Text(order.OrderTotal).ToString();
                                table.Cell().Text(order.Discount).ToString();
                            }
                        });
                    });

                    page.Footer().Text(text =>
                    {
                        text.Span("Page :");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf(Path.Combine(appPath, fileName));

        }
    }
}
