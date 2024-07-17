
namespace EasyLibs
{
    public interface IBindingWidget
    {
        void Bind(IBindingData bindingObject, string bindingField);

        void Unbind();
    }
}