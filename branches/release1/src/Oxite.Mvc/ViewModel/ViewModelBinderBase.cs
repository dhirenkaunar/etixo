namespace Oxite.Mvc.ViewModel
{
    using System;
    using System.Web.Mvc;

    public abstract class ViewModelBinderBase<TViewModel> : IViewModelBinder
        where TViewModel : ViewModelBase
    {
        #region IViewModelBinder Members

        public Type ViewModelType
        {
            get { return typeof (TViewModel); }
        }

        public ModelBinderResult BindModel(ModelBindingContext bindingContext)
        {
            return bindingContext.ModelType != ViewModelType ? null : PerformBind(bindingContext);
        }

        #endregion

        public abstract ModelBinderResult PerformBind(ModelBindingContext bindingContext);
    }
}