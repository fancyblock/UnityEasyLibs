using System;

namespace EasyLibs
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }

        protected Singleton()
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Cannot create multiple instances of a Singleton.");
            }

            instance = (T)this;
        }
    }
}