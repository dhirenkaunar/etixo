namespace Oxite.Mvc.Services
{
    using System.Configuration;
    using System.Web;
    using System.Web.Security;
    using Configuration;
    using Data;
    using ViewModel.Security;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMembershipRepository membershipRepository;

        public AuthenticationService()
            : this((IOxiteConfiguration) ConfigurationManager.GetSection("oxite"))
        {
        }

        public AuthenticationService(IOxiteConfiguration configuration)
            : this(configuration.DataProvider.GetInstance())
        {
        }

        public AuthenticationService(IOxiteDataProvider provider)
            : this(provider.MembershipRepository)
        {
        }

        public AuthenticationService(IMembershipRepository repository)
        {
            membershipRepository = repository;
        }

        #region IAuthenticationService Members

        public AuthenticationResultViewModel Authenticate(CredentialViewModel viewModel)
        {
            var result = new AuthenticationResultViewModel();

            if (viewModel == null)
            {
                result.Message = "Please provide valid credentials.";
                result.Status = AuthenticationStatus.Failed;

                return result;
            }

            IUser user = membershipRepository.GetUser(viewModel.Username, viewModel.Password);
            result.Status = user != null ? AuthenticationStatus.Success : AuthenticationStatus.Failed;
            ;

            return result;
        }

        public void SetSecurityContext(CredentialViewModel credentialViewModel)
        {
            FormsAuthentication.SetAuthCookie(credentialViewModel.Username, credentialViewModel.Persist);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        public bool IsAuthenticated()
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity != null)
                {
                    return HttpContext.Current.User.Identity.IsAuthenticated;
                }
            }

            return false;
        }

        #endregion
    }
}