namespace PORLA.HMI.Module.DataService.DataHandle
{
    public interface IDbObject
    {
        string GetInsertQuery();
        string GetUpdateQuery();
    }
}