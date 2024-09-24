namespace Demo.Filter;

public class CustomErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        /*if (error.Exception is NotFoundException notFoundException)
        {
            return error.WithCode(notFoundException.Code);
        }*/

        return error;
    }
}
