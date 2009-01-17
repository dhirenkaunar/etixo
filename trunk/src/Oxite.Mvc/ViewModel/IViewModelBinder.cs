namespace Oxite.Mvc.ViewModel
{
    using System;
    using System.Web.Mvc;

    public interface IViewModelBinder : IModelBinder
    {
        Type ViewModelType { get; }
    }
}