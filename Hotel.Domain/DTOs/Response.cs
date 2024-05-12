using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs;

public class Response<T> : IDataTransferObject
{
  public Response(int status, string message, T data)
  {
    Status = status;
    Message = message;
    Data = data;
  }

  public Response(int status, List<string> errors)
  {
    Status = status;
    Errors = errors;
  }

  public Response(int status,  string error)
  {
    Status = status;
    Errors.Add(error);
  }

  public int Status { get; private set; }
  public string Message { get; private set; } = "";
  public T? Data { get; private set; }
  public List<string> Errors { get; private set; } = [];
}