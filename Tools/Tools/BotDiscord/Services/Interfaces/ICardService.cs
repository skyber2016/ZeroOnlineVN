using API.Entities;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface ICardService : IGeneralService<CardEntity>
    {
        Task<CardEntity> CardCharge(CardEntity card);
    }
}
