using System;

namespace BLink.Models.Enums
{
    [Flags]
    public enum  PlayerStatus
    {
        All = 0,
        Fit = 1,
        Injured = 1 << 1
    }
}
