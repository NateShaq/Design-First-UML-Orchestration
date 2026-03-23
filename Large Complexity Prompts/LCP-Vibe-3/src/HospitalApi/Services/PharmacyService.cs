using HospitalApi.Data;
using HospitalApi.Domain;

namespace HospitalApi.Services;

public class PharmacyService
{
    private readonly HospitalRepository _repo;

    public PharmacyService(HospitalRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<PharmacyItem> GetInventory() => _repo.PharmacyInventory;

    public DispenseEvent Dispense(DispenseEvent request)
    {
        var item = _repo.PharmacyInventory.FirstOrDefault(i => i.Id == request.PharmacyItemId);
        if (item != null && item.QuantityOnHand >= request.Quantity)
        {
            item.QuantityOnHand -= request.Quantity;
        }
        request.Id = Guid.NewGuid();
        request.DispensedAt = DateTime.UtcNow;
        return request;
    }
}
