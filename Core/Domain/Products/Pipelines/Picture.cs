public class Picture
{
    public string Path { get; private set; } // Relative path to the file

    private Picture(string path)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public static Picture Default()
    {
        return new Picture("/images/fooditems/default.jpg"); // Path to default picture
    }

    public static Picture FromPath(string path)
    {
        return new Picture(path);
    }
}