using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.PaymentContext;
using Hotel.Domain.Repositories.Interfaces.ReservationContext;
using Hotel.Domain.Services.EmailServices.Interface;
using Hotel.Domain.Services.EmailServices.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;


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
    try
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
      var verdana12 = new XFont("Verdana", 12, XFontStyleEx.Regular);


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

      gfx.DrawString(invoice.Customer!.Name.GetFullName(), boldFont10, XBrushes.Black, //Valor da primeira linha
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
      gfx.DrawString("1", verdana12, XBrushes.Black,
        new XRect(
          marginX + 10,
          keyYSpacing + 180 + lineSpacing,
          page.Width.Point,
          page.Height.Point),
        XStringFormats.TopLeft);

      gfx.DrawString($"Hospedagem", verdana12, XBrushes.Black,
        new XRect(
          marginX + 40,
          keyYSpacing + 180 + lineSpacing,
          page.Width.Point,
          page.Height.Point),
        XStringFormats.TopLeft);

      gfx.DrawString($"R${reservation.DailyRate}", verdana12, XBrushes.Black,
        new XRect(
          marginX + 200,
          keyYSpacing + 180 + lineSpacing,
          page.Width.Point,
          page.Height.Point),
        XStringFormats.TopLeft);

      gfx.DrawString($"R${invoice.TotalAmount}", verdana12, XBrushes.Black,
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
        gfx.DrawString(serviceCount.ToString(), verdana12, XBrushes.Black,
        new XRect(
          marginX + 10,
          keyYSpacing + 180 + lineSpacing + currentService * lineSpacing,
          page.Width.Point,
          page.Height.Point),
        XStringFormats.TopLeft);

        gfx.DrawString(service.Name, verdana12, XBrushes.Black,
          new XRect(
            marginX + 40,
            keyYSpacing + 180 + lineSpacing + currentService * lineSpacing,
            page.Width.Point,
            page.Height.Point),
          XStringFormats.TopLeft);

        gfx.DrawString($"R${service.Price}", verdana12, XBrushes.Black,
          new XRect(
            marginX + 200,
            keyYSpacing + 180 + lineSpacing + currentService * lineSpacing,
            page.Width.Point,
            page.Height.Point),
          XStringFormats.TopLeft);

        gfx.DrawString($"R${service.Price * serviceCount}", verdana12, XBrushes.Black,
          new XRect(
            marginX + 350,
            keyYSpacing + 180 + lineSpacing + currentService * lineSpacing,
            page.Width.Point,
            page.Height.Point),
          XStringFormats.TopLeft);
      }

      gfx.DrawString($"Você tem um prazo de 15 dias para realizar o pagamento.", boldFont14, XBrushes.Black,
       new XRect(
         marginX,
         300,
         page.Width.Point,
         page.Height.Point),
       XStringFormats.TopLeft);


      var directoryPath = "wwwroot/PdfsExample";
      Directory.CreateDirectory(directoryPath);

      var relativePath = $"{directoryPath}/pdfexample-{new Random().Next(10000)}.pdf";
      var absolutePath = Path.GetFullPath(relativePath);

      document.Save(absolutePath);

   
      var email = new SendEmailModel(invoice.Customer.Email, "Fatura", "Muito obrigado pela preferência. Aqui está sua fatura:", absolutePath);
      await _emailService.SendEmailAsync(email);
     
      await _repository.CreateAsync(invoice);
      await _repository.SaveChangesAsync();

      return new Response(200, "Fatura de quarto criada com sucesso!");
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      throw;
    }
    
  }
}