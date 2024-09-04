namespace UserManagement.UnitTests.MockData
{
    public interface IUserData<TResponse>
    {
        TResponse GetData();
    }
}
