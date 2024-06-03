using Light.Mail.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Graph
{
    public interface IGraphMailService
    {
        Task SendAsync(
            Sender sender,
            Mail.Contracts.MailMessage mail,
            CancellationToken cancellationToken = default);
    }
}