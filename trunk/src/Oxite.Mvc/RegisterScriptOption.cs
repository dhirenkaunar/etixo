using System;

namespace Oxite.Mvc
{
    [Flags]
    public enum RegisterScriptOption : byte
    {
        NotSet = 0,
        PlaceInHead = 1,
        LoadFromSkin = 2
    }
}
