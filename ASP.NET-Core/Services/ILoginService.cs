namespace AspNetCoreJWT.Services
{
    using AspNetCoreJWT.Dto;

    public interface ILoginService
    {
        bool Authenticate(UserLogin user);

        string GenerateJWT(UserLogin user);
    }
}
