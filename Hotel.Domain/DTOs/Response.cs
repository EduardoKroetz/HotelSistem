namespace Hotel.Domain.DTOs;

public class Response<T> : IDataTransferObject
{

    public Response(string message, T data)
    {
        Message = message;
        Data = data;
    }

    public Response(string message)
    {
        Message = message;
    }

    public Response(List<string> errors)
    {
        Errors = errors;
    }

    public string Message { get; private set; } = "";
    public T? Data { get; private set; }
    public List<string> Errors { get; private set; } = [];
}

public class Response : IDataTransferObject
{
    private Response() { }

    public Response(string message, object data)
    {
        Message = message;
        Data = data;
    }

    public Response(string message)
    {
        Message = message;
    }

    public Response(List<string> errors)
    {
        Errors = errors;
    }

    public string Message { get; private set; } = "";
    public object? Data { get; private set; }
    public List<string> Errors { get; private set; } = [];
}