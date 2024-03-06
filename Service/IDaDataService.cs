using Domain.Entities;

namespace Service;

public interface IDaDataService
{
    Task<Address> GetJsonFromDaData(AddressDto address);
}