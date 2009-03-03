using System;
using System.Configuration;

namespace Oxite.Configuration
{
    public class SiteConfigurationElementCollection : ConfigurationElementCollection, ISiteRedirectCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SiteRedirectConfigurationElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new SiteRedirectConfigurationElement(elementName);
        }

        protected new SiteRedirectConfigurationElement BaseGet(int index)
        {
            return (SiteRedirectConfigurationElement)base.BaseGet(index);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SiteRedirectConfigurationElement)element).Name;
        }
    }
}
