namespace Api.Utils;

public static class PageHelper
{
    public static int GetTotalPages(int total, int pageSize)
    {
        return (int)Math.Ceiling(total / (decimal)pageSize);
    }
}
