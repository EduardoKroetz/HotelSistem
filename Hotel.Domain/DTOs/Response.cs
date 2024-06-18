namespace Hotel.Domain.DTOs;

public class Response<T> : IDataTransferObject
{

    public Response(int status, string message, T data)
    {
        Status = status;
        Message = message;
        Data = data;
    }

    public Response(int status, string message)
    {
        Status = status;
        Message = message;
    }

    public Response(int status, List<string> errors)
    {
        Status = status;
        Errors = errors;
    }

    public int Status { get; private set; }
    public string Message { get; private set; } = "";
    public T? Data { get; private set; }
    public List<string> Errors { get; private set; } = [];
}

public class Response : IDataTransferObject
{
    private Response() { }

    public Response(int status, string message, object data)
    {
        Status = status;
        Message = message;
        Data = data;
    }

    public Response(int status, string message)
    {
        Status = status;
        Message = message;
    }

    public Response(int status, List<string> errors)
    {
        Status = status;
        Errors = errors;
    }

    public int Status { get; private set; }
    public string Message { get; private set; } = "";
    public object? Data { get; private set; }
    public List<string> Errors { get; private set; } = [];
}