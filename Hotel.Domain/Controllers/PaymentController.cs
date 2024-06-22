using Hotel.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace Hotel.Domain.Controllers;

[ApiController]
[Route("v1/payments")]
public class PaymentController : ControllerBase
{
    [HttpPost("checkout/card/{priceId}")]
    public async Task<IActionResult> CreateCheckout([FromRoute] string priceId)
    {
        if (string.IsNullOrEmpty(priceId))
        {
            return BadRequest(new Response("ID de preço inválido"));
        }

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card"
            },

            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1,
                }
            },
            Mode = "payment",
            SuccessUrl = "https://localhost:7100/success.html",
            CancelUrl = "https://localhost:7100/cancel.html"
        };

        var service = new SessionService();

        try
        {
            var session = await service.CreateAsync(options);

            return Ok(new Response("Sessão criada com sucesso!", new { sessionId = session.Id }));
        }
        catch (StripeException)
        {
            throw new StripeException("Algum erro ocorreu ao criar a sessão de pagamento");
        }
    }
}
