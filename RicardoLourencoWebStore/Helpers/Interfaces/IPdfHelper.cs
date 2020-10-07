using Microsoft.AspNetCore.Mvc;
using RicardoLourencoWebStore.Data.Entities;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Helpers.Interfaces
{
    public interface IPdfHelper
    {
        Task<FileStreamResult> GenerateBillPDFAsync(Order order);

        PdfLayoutResult BodyContent(string text, float yPosition, PdfPage page);

        PdfLayoutResult HeaderPoints(string text, float yPosition, PdfPage page);
    }
}
