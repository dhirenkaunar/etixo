namespace Oxite.Mvc.ViewModel.Security
{
    using System;

    [Serializable]
    public class CredentialViewModel : ViewModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Persist { get; set; }
    }
}