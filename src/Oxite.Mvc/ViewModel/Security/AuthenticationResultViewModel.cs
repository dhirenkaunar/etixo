namespace Oxite.Mvc.ViewModel.Security
{
    using System;

    [Serializable]
    public class AuthenticationResultViewModel : ViewModelBase
    {
        public string Message { get; set; }
        public AuthenticationStatus Status { get; set; }
        public bool IsInitialLogin { get; set; }
    }
}