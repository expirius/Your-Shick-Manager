using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker;

public interface IGeolocator
{

    Task StartListening(IProgress<Location> positionChangedProgress, CancellationToken cancellationToken);
}
