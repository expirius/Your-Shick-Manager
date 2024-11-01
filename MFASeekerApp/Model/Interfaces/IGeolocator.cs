using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeekerApp.Model.Interfaces;

public interface IGeolocator
{

    Task StartListening(IProgress<Location> positionChangedProgress, CancellationToken cancellationToken);
    void StopListening();

}
