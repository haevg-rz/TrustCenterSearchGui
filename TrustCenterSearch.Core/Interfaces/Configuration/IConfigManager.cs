using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.Interfaces.Configuration
{
    internal interface IConfigManager
    {
        Config LoadConfig();

        Config AddTrustCenterToConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config);

        Config SaveConfig(Config config);

        Config DeleteTrustCenterFromConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config);

        bool IsConfigEmpty(Config config);
    }
}
