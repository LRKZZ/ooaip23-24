namespace spacebattle;

public interface ISender
{
    public void Send(ICommand message);
}