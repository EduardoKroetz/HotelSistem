﻿using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReportHandlers;

public partial class ReportHandler
{
    public async Task<Response> HandleFinishAsync(Guid id)
    {
        var report = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Relatório não encontrado.");

        report.Finish();

        await _repository.SaveChangesAsync();
        return new Response("Relatório finalizado com sucesso!", new { report.Id });
    }
}