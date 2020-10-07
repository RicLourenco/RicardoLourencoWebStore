using RicardoLourencoWebStore.Helpers.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Mvc;
using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Data;
using Microsoft.EntityFrameworkCore;

namespace RicardoLourencoWebStore.Helpers.Classes
{
    public class PdfHelper : IPdfHelper
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly DataContext _context;

        public PdfHelper(
            IHostingEnvironment hostingEnvironment,
            DataContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        Color aliceBlue = Color.AliceBlue;
        Color blue = Color.CornflowerBlue;

        public async Task<FileStreamResult> GenerateBillPDFAsync(Order order)
        {
            //Creating new PDF document instance
            PdfDocument document = new PdfDocument();

            //Setting margins
            document.PageSettings.Margins.All = 0;

            //Adding a new page
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            //Creating font instances
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 35);
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 16);

            //Drawing content onto the PDF
            graphics.DrawRectangle(new PdfSolidBrush(aliceBlue), new Rectangle(0, 0, (int)page.Graphics.ClientSize.Width, (int)page.Graphics.ClientSize.Height));
            graphics.DrawRectangle(new PdfSolidBrush(blue), new Rectangle(0, 0, (int)page.Graphics.ClientSize.Width, 100));
            graphics.DrawString("RicardoLourencoWebStore", headerFont, new PdfSolidBrush(aliceBlue), new PointF(10, 20));

            PdfLayoutResult result = HeaderPoints("To:", 130, page);
            result = HeaderPoints($"Name: {order.User.FullName}", result.Bounds.Bottom + 10, page);
            result = HeaderPoints($"Address: {order.User.Address}", result.Bounds.Bottom + 10, page);

            result = BodyContent("Order info:", result.Bounds.Bottom + 25, page);
            result = BodyContent($"Serial number: {order.Id}", result.Bounds.Bottom + 10, page);
            result = BodyContent("Invoice info:", result.Bounds.Bottom + 45, page);

            Product product;

            foreach(var orderDetail in order.Items)
            {
                product = await _context.Products.FindAsync(orderDetail.ProductId);

                result = BodyContent($"Product name: {orderDetail.Product.Name}", result.Bounds.Bottom + 30, page);
                result = BodyContent($"Quantity: {orderDetail.Quantity}", result.Bounds.Bottom + 10, page);
                result = BodyContent($"Price: {orderDetail.Value}", result.Bounds.Bottom + 10, page);
            }

            graphics.DrawString("All trademarks mentioned belong to their owners.", new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Italic), PdfBrushes.White, new PointF(10, graphics.ClientSize.Height - 30));
            PdfTextWebLink linkAnnot = new PdfTextWebLink();
            linkAnnot.Url = "//www.syncfusion.com";
            linkAnnot.Text = "www.syncfusion.com";
            linkAnnot.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Italic);
            linkAnnot.Brush = PdfBrushes.White;
            linkAnnot.DrawTextWebLink(page, new PointF(graphics.ClientSize.Width - 100, graphics.ClientSize.Height - 30));

            //Saving the PDF to the MemoryStream
            MemoryStream ms = new MemoryStream();
            document.Save(ms);
            //If the position is not set to '0' then the PDF will be empty.
            ms.Position = 0;

            //Download the PDF document in the browser.
            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf")
            {
                FileDownloadName = $"Invoice_{order.Id}_{DateTime.UtcNow}.pdf"
            };

            return fileStreamResult;
        }

        public PdfLayoutResult HeaderPoints(string text, float yPosition, PdfPage page)
        {
            float headerBulletsXposition = 35;
            PdfTextElement txtElement = new PdfTextElement("")
            {
                Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 13),
                Brush = new PdfSolidBrush(aliceBlue),
                StringFormat = new PdfStringFormat()
            };

            txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
            txtElement.StringFormat.LineLimit = true;
            txtElement.Draw(page, new RectangleF(headerBulletsXposition, yPosition, 300, 100));


            txtElement = new PdfTextElement(text)
            {
                Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11),
                StringFormat = new PdfStringFormat()
            };

            txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
            txtElement.StringFormat.LineLimit = true;
            PdfLayoutResult result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 20, yPosition, 320, 100));
            return result;
        }

        public PdfLayoutResult BodyContent(string text, float yPosition, PdfPage page)
        {
            float headerBulletsXposition = 35;
            PdfTextElement txtElement = new PdfTextElement("")
            {
                Font = new PdfStandardFont(PdfFontFamily.ZapfDingbats, 16),
                StringFormat = new PdfStringFormat()
            };

            txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
            txtElement.StringFormat.LineLimit = true;
            txtElement.Draw(page, new RectangleF(headerBulletsXposition, yPosition, 320, 100));


            txtElement = new PdfTextElement(text)
            {
                Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 17),
                StringFormat = new PdfStringFormat()
            };

            txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
            txtElement.StringFormat.LineLimit = true;
            PdfLayoutResult result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 25, yPosition, 450, 130));
            return result;
        }
    }
}
