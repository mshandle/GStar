namespace Framework
{
    public class BaseComponentTemplate<T> : BaseComponent where T : BaseComponentTemplate<T>
    {
        protected static T instance = null;
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        public static T RegisterComponent(T compoent)
        {
            instance = compoent;
            return instance;
        }

    }

}

