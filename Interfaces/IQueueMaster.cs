namespace AargonTools.Interfaces
{
    public interface IQueueMaster
    {
         int? Employee { get; set; }
         int? Priority { get; set; }
         string DebtorAcct { get; set; }
    }
}
