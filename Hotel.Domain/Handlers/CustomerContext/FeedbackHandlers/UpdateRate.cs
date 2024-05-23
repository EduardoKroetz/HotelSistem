﻿using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response<object>> HandleUpdateRateAsync(Guid id, int rate)
  {
    var feedback = await _repository.GetEntityByIdAsync(id);
    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    feedback.ChangeRate(rate);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Feedback atualizado.");
  }
}