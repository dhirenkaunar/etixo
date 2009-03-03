namespace Oxite.Mvc.ViewModel
{
    using System;

    [Serializable]
    public abstract class ViewModelBase
    {
        public string ViewModelName
        {
            get { return GetType().Name; }
        }
    }
}