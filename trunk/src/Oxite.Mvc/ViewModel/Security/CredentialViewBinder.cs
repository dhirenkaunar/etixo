namespace Oxite.Mvc.ViewModel.Security
{
    using System.Collections.Specialized;
    using System.Web.Mvc;

    public class CredentialViewBinder : ViewModelBinderBase<CredentialViewModel>
    {
        public override ModelBinderResult PerformBind(ModelBindingContext bindingContext)
        {
            NameValueCollection form = bindingContext.HttpContext.Request.Form;
            string username = form["username"];
            string password = form["password"];
            bool rememberMe = bool.Parse(form["rememberMe"] ?? "false");

            if (string.IsNullOrEmpty(username))
            {
                bindingContext.ModelState.AddModelError("username", "You must specify a username.");
            }

            if (string.IsNullOrEmpty(password))
            {
                bindingContext.ModelState.AddModelError("password", "You must specify a password.");
            }

            if (!bindingContext.ModelState.IsValid)
            {
                return null;
            }

            var credential = new CredentialViewModel
                                 {
                                     Username = username,
                                     Password = password,
                                     Persist = rememberMe
                                 };

            return new ModelBinderResult(credential);
        }
    }
}