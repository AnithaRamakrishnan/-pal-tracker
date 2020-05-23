using System;
using Steeltoe.Management.Endpoint.Info;

namespace PalTracker
{
    public interface IInfoContributor
    {
        void Contribute(IInfoBuilder builder);
    }
}
