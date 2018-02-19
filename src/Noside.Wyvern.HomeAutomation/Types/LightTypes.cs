using System;
using System.ComponentModel;

namespace Noside.Wyvern.HomeAutomation.Types
{
    [Flags]
    enum LightTypes {
        [Description("on/off light")]
        OnOffLight = 1,
        [Description("dimmable light")]
        DimmableLight = 2,
        [Description("color Temperature Ligh")]
        TempatureLight = 4,
        [Description("color light")]
        ColorLight = 8,
        [Description("extended color light")]
        ExtendedColorLight = 16,

        //Extended Types
        OnOff = OnOffLight | DimmableLight | TempatureLight | ColorLight | ExtendedColorLight,
        Dimmable = DimmableLight | TempatureLight | ColorLight| ExtendedColorLight,
        Tempature = TempatureLight | ColorLight| ExtendedColorLight,
        RGB = ColorLight | ExtendedColorLight
    }
}