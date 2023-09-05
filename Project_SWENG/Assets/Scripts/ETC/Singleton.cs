public class Singleton<T> where T : new()
{
    protected static T instance;

    protected static T GetInstance()
    {
        if (instance == null)
        {
            instance = new T();
        }
        return instance;
    }
}