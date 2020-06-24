using System;
using System.Collections.Generic;
using System.Text;

namespace TrustCenterSearchGui.Core.Models
{
    public class TrustCenterHistoryElement
    {
        public string ImageAddPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                   @"\TrustCenterSearch\images\add.png";
        public string ImageBinBlackPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                   @"\TrustCenterSearch\images\binBlack.png";
        public string TrustCenterName { get; set; }

        public TrustCenterHistoryElement(string trustCenterName)
        {
            this.TrustCenterName = trustCenterName;
        }
    }
}
