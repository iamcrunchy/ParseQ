namespace ParseQ.Helpers;

public static class ReadFormFile
{
    public static async Task<string> ReadAllBytesAsync(this IFormFile file)
    {
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            return await reader.ReadToEndAsync();
        }
    }
}