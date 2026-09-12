using Crossdyne.Toolkit.Results;
using Shared.Kernel.Exceptions;

namespace Connectome.SocialGraph.Domain.Exceptions
{
    public sealed class EmptyValueException(Error error) : DomainException(error)
    {
        
    }
}