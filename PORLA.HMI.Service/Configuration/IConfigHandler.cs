namespace PORLA.HMI.Service.Configuration
{
    public interface IConfigHandler
    {
        T GetValue<T>(string name);
        bool GetValue<T>(string name, out T value);
        bool GetValue<T>(string name, out T value, out string errorCode);
        bool LoadXML(string xmlPath);
        bool SetValue<T>(string name, T value);
        bool SetValue<T>(string name, T value, out string errorCode);
        bool CreateNewElement<T>(string name, T value);
        bool CreateNewElement<T>(string name, T value, out string errorCode);
        bool CreateNewHeaderElement(string name);
        bool AppendToRoot();
        bool SaveXmlFile();
        bool UpdateInnerSingleNode<T>(string name, T value);
        bool CheckNode(string path, string nameNode, string condition);
        bool DeleteNode();
        bool RemoveNode(string condition);
    }
}