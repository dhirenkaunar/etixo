using System;

namespace Oxite.Configuration
{
    public interface ISiteRedirect
    {
        string Name { get; set; }
        string Host { get; set; }
    }
}
