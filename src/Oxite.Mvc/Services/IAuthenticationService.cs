namespace Oxite.Mvc.Services
{
    using ViewModel.Security;

    public interface IAuthenticationService
    {
        AuthenticationResultViewModel Authenticate(CredentialViewModel viewModel);
        void SetSecurityContext(CredentialViewModel credentialViewModel);
        void Logout();
        bool IsAuthenticated();
    }
}