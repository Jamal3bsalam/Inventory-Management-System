using Inventory.Mostafa.Domain.Entities.AssetsReturns;

namespace Inventory.Mostafa.Domain.Specification.Return
{
    public class ReturnItemSpec : Specifications<ReturnItem, int>
    {
        public ReturnItemSpec(int id) : base(i => i.ReturnId == id)
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(r => r.UnitExpense);
            Include.Add(r => r.Item);
        }
    }
}
