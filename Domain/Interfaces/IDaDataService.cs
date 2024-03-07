using Domain.Models;

namespace Domain.Interfaces;

public interface IDaDataService
{
    Task<Address> GetJsonFromDaData(AddressDto address);
}