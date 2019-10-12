using System.ComponentModel;

namespace Infrastructure.Wrapper
{
    public interface IModelWrapper<T> : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        T Model { get; set; }
    }
}