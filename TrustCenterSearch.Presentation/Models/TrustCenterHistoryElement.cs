using GalaSoft.MvvmLight;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Presentation.Models
{
    public class TrustCenterHistoryElement : ViewModelBase
    {
        public TrustCenterMetaInfo TrustCenterMetaInfo { get; set; }

        private bool _active = true;

        public bool Active
        {
            get => this._active;
            set => Set(() => this.Active, ref this._active, value);
        }

        public TrustCenterHistoryElement(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            TrustCenterMetaInfo = trustCenterMetaInfo;
        }
    }
}
