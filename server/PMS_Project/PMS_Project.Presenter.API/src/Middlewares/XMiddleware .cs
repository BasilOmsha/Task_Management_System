namespace PMS_Project.Presenter.API.Middlewares
{
    public class XMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
