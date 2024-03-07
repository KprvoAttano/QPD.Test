using Domain.Models;

namespace Domain.Interfaces;

public interface IDaDataService
{
    Task<List<Address>> GetJsonFromDaDataAsync(AddressDto address, CancellationToken token);
}