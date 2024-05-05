namespace Hotel.Domain.DTOs;

public class Response<T> 
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

  public int Status { get; set; }
  public string Message { get; set; } = "";
  public T? Data { get; set; }
  public List<string> Errors { get; set; } = [];
}