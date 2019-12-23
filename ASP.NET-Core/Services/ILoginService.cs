namespace ASPNETCore.Services
{
    using ASPNETCore.Dto;

    /// <summary>
    /// Interface used to login to application
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// determiante if a user is allowed to login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool Authenticate(UserLogin user);

        /// <summary>
        /// Generate JWT auth token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateJWT(UserLogin user);
    }
}
