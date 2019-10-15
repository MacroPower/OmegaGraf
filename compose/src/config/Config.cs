namespace OmegaGraf.Compose.Config
{
    public class Config<T>
    {
        public string Path { get; set; }
        public T Data { get; set; }
    }
}