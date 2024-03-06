using Domain.Entities;

namespace Domain.Interfaces;

public interface IDaDataService
{
    Task<Address> GetJsonFromDaData(AddressDto address);
}