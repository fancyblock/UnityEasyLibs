
namespace EasyLibs
{
    public class BaseCmd
    {
        [Inject] protected SignalBus m_bus;


        public object SIGNAL { get; set; }

        public virtual void Exec() { }
    }
}