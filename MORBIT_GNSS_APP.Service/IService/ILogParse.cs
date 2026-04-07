namespace MORBIT_GNSS_APP.Service.IService
{
    public interface ILogParse
    {
        bool IsLogParse(string line, ref INmeaBaseModel model);
    }
}
