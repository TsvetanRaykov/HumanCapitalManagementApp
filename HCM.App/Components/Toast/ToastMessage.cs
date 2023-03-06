namespace HCM.App.Components.Toast;

public class ToastMessage
{
    public string Title { get; set; } = "Notification";
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public ToastMessage(string? message)
    {
        Message = message;
    }
}