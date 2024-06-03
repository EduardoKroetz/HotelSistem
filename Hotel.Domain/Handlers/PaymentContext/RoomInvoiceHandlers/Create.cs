using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.PaymentContext;
using Hotel.Domain.Repositories.Interfaces.ReservationContext;
using Hotel.Domain.Services.EmailServices.Interface;
using Hotel.Domain.Services.EmailServices.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.UniversalAccessibility.Drawing;
using System.Diagnostics;
using System.Net;


namespace Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;

public partial class RoomInvoiceHandler : IHandler
{
  private readonly IRoomInvoiceRepository  _repository;
  private readonly IReservationRepository  _reservationRepository;
  private readonly IEmailService _emailService;
  public RoomInvoiceHandler(IRoomInvoiceRepository repository, IReservationRepository reservationRepository, IEmailService emailService)
  {
    _repository = repository;
    _reservationRepository = reservationRepository;
    _emailService = emailService;
  }


  public async Task<Response> HandleCreateAsync(RoomInvoice invoice, Reservation reservation)
  {
    var document = new PdfDocument();
    document.Info.Title = "Fatura";

    var marginTop = 40;
    var marginX = 30;
    var lineSpacing = 25;
    var valueSpacing = 190;
    var keyYSpacing = marginTop + lineSpacing;

    var page = document.AddPage();
    page.Width = XUnit.FromMillimeter(200);
    page.Height = XUnit.FromMillimeter(297);

    var gfx = XGraphics.FromPdfPage(page);

    var boldFont14 = new XFont("Verdana", 14, XFontStyleEx.Bold);
    var boldFont10 = new XFont("Verdana", 10, XFontStyleEx.Bold);
    var arialFont = new XFont("Arial", 12, XFontStyleEx.Regular);


    //HEADER
    gfx.DrawString("Hotel Sistem", boldFont14, XBrushes.Black,
      new XRect(
        marginX,
        marginTop, page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    gfx.DrawString("FATURA", boldFont14, XBrushes.Black,
      new XRect(
        -marginX,
        marginTop, 
        page.Width.Point, 
        page.Height.Point),
      XStringFormats.TopRight);

    //PRIMEIRA LINHA - COBRAR A
    gfx.DrawString("COBRAR A", boldFont10, XBrushes.Black, //Chave da primeira linha
      new XRect(
        marginX,
        keyYSpacing + 40,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    gfx.DrawString("Reginaldo Campos", boldFont10, XBrushes.Black, //Valor da primeira linha
      new XRect(
        valueSpacing,
        keyYSpacing + 40,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    //SEGUNDA LINHA - DATA DA EMISSÂO
    gfx.DrawString("DATA DA EMISSÂO", boldFont10, XBrushes.Black, // Chave segunda linha
      new XRect(
        marginX,
        keyYSpacing + 60,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    gfx.DrawString(invoice.CreatedAt.ToString(), boldFont10, XBrushes.Black, // Valor segunda linha
      new XRect(
        valueSpacing,
        keyYSpacing + 60,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    //TERCEIRA LINHA - MÉTODO DE PAGAMENTO
    gfx.DrawString("MÉTODO DE PAGAMENTO", boldFont10, XBrushes.Black, // Chave
      new XRect(
        marginX,
        keyYSpacing + 80,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    gfx.DrawString(invoice.PaymentMethod.ToString(), boldFont10, XBrushes.Black, // Valor
      new XRect(
        valueSpacing,
        keyYSpacing + 80,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);


    //TOTAL DA FATURA
    gfx.DrawString("TOTAL DA FATURA", boldFont14, XBrushes.Black,
      new XRect(
        marginX,
        keyYSpacing + 120,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    gfx.DrawString($"R${invoice.TotalAmount}", boldFont14, XBrushes.Black,
      new XRect(
        valueSpacing,
        keyYSpacing + 120,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    //LISTA DE SERVIÇOS
    gfx.DrawString("SERVIÇOS", boldFont10, XBrushes.Black,
      new XRect(
        marginX,
        keyYSpacing + 160,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);


    //SERVIÇOS - 1 LINHA = QTD
    gfx.DrawString("QTD", boldFont10, XBrushes.Black,
       new XRect(
         marginX + 10,
         keyYSpacing + 180,
         page.Width.Point,
         page.Height.Point),
       XStringFormats.TopLeft);

    //SERVIÇOS - 1 LINHA = NOME
    gfx.DrawString("NOME", boldFont10, XBrushes.Black,
       new XRect(
         marginX + 40,
         keyYSpacing + 180,
         page.Width.Point,
         page.Height.Point),
       XStringFormats.TopLeft);

    //SERVIÇOS - 1 LINHA = PREÇO POR UNIDADE
    gfx.DrawString("PREÇO POR UNIDADE", boldFont10, XBrushes.Black,
       new XRect(
         marginX + 200,
         keyYSpacing + 180,
          page.Width.Point,
         page.Height.Point),
       XStringFormats.TopLeft);

    gfx.DrawString("VALOR", boldFont10, XBrushes.Black,
     new XRect(
       marginX + 350,
       keyYSpacing + 180,
       page.Width.Point,
       page.Height.Point),
     XStringFormats.TopLeft);

    //Serviço padrão
    gfx.DrawString("1", arialFont, XBrushes.Black,
      new XRect(
        marginX + 10,
        keyYSpacing + 180 + lineSpacing,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    gfx.DrawString($"Hospedagem por {reservation.HostedDays} dias.", arialFont, XBrushes.Black,
      new XRect(
        marginX + 40,
        keyYSpacing + 180 + lineSpacing,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    gfx.DrawString($"R${reservation.DailyRate}", arialFont, XBrushes.Black,
      new XRect(
        marginX + 200,
        keyYSpacing + 180 + lineSpacing,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    gfx.DrawString($"R${invoice.TotalAmount}", arialFont, XBrushes.Black,
      new XRect(
        marginX + 350,
        keyYSpacing + 180 + lineSpacing,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

    var currentService = 0;
    foreach (var service in invoice.Services)
    {
      currentService++;
      var serviceCount = invoice.Services.Count(x => x == service);

      //QTD
      gfx.DrawString(serviceCount.ToString(), arialFont, XBrushes.Black,
      new XRect(
        marginX + 10,
        keyYSpacing + 180 + lineSpacing + currentService * lineSpacing,
        page.Width.Point,
        page.Height.Point),
      XStringFormats.TopLeft);

      gfx.DrawString(service.Name, arialFont, XBrushes.Black,
        new XRect(
          marginX + 40,
          keyYSpacing + 180 + lineSpacing + currentService * lineSpacing,
          page.Width.Point,
          page.Height.Point),
        XStringFormats.TopLeft);

      gfx.DrawString($"R${service.Price}", arialFont, XBrushes.Black,
        new XRect(
          marginX + 200,
          keyYSpacing + 180 + lineSpacing + currentService * lineSpacing,
          page.Width.Point,
          page.Height.Point),
        XStringFormats.TopLeft);

      gfx.DrawString($"R${service.Price * serviceCount}", arialFont, XBrushes.Black,
        new XRect(
          marginX + 350,
          keyYSpacing + 180 + lineSpacing + currentService * lineSpacing,
          page.Width.Point,
          page.Height.Point),
        XStringFormats.TopLeft);
    }


    var directoryPath = "wwwroot/PdfsExample";
    Directory.CreateDirectory(directoryPath);

    var relativePath = $"{directoryPath}/pdfexample-{new Random().Next(10000)}.pdf";
    var absolutePath = Path.GetFullPath(relativePath);

    document.Save(absolutePath);

    Process.Start(new ProcessStartInfo(absolutePath) { UseShellExecute = true });

    return new Response(200,"Fatura de quarto criada com sucesso!");
  }
}