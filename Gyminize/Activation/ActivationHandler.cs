namespace Gyminize.Activation;

// Lớp cơ sở trừu tượng để triển khai các ActivationHandler mới. Xem DefaultActivationHandler để biết ví dụ.
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
public abstract class ActivationHandler<T> : IActivationHandler
    where T : class
{
    // Ghi đè phương thức này để thêm logic cho việc xử lý kích hoạt.
    protected virtual bool CanHandleInternal(T args) => true;

    // Ghi đè phương thức này để thêm logic cho trình xử lý kích hoạt của bạn.
    protected abstract Task HandleInternalAsync(T args);

    public bool CanHandle(object args) => args is T && CanHandleInternal((args as T)!);

    public async Task HandleAsync(object args) => await HandleInternalAsync((args as T)!);
}