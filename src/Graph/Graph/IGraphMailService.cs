using Light.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Graph
{
    public interface IGraphMailService
    {
        Task SendAsync(MailMessage mail, CancellationToken cancellationToken = default);
    }
}