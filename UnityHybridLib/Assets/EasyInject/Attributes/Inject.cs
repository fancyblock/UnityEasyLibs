using System;

namespace EasyLibs
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Inject : System.Attribute
    {
        public Inject() 
        {
            Id = null;
        }

        public Inject(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
