using System;
using System.Collections.Generic;
using System.Text;
using TrustCenterSearch.Core.Interfaces.Configuration;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearchCore.Test.MockObjects
{
    public class IConfigManagerTest : IConfigManager
    {
        public Config LoadConfig()
        {
            throw new NotImplementedException();
        }

        public void AddTrustCenterToConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config)
        {
            throw new NotImplementedException();
        }

        public void SaveConfig(Config config)
        {
            return;
        }

        public void DeleteTrustCenterFromConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config)
        {
            throw new NotImplementedException();
        }

        public bool IsConfigEmpty(Config config)
        {
            throw new NotImplementedException();
        }
    }
}
