
public class Event 
{
    public delegate void EventHandler();
    public event EventHandler myEvent;

    public object sender;

    public void CallEvent(object sender)
    {
        this.sender = sender;
        myEvent();
    }
}
