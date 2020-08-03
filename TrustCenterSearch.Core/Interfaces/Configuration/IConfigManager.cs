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

        void AddTrustCenterToConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config);

        void SaveConfig(Config config);

        void DeleteTrustCenterFromConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config);

        bool IsConfigEmpty(Config config);
    }
}
