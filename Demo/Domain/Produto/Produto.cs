namespace Demo.Domain.Produto
{
    public class Produto
    {
        public Guid Id { get; set; }
        public String Nome { get; set; } = null!;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Nome);
        }  
    }
}
