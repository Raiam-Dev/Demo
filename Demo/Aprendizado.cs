using Microsoft.EntityFrameworkCore.Query;

namespace Demo
{
    public class Aprendizado
    {
        public DateTime validTo => DateTime.UtcNow; // Propiedade Somente Leitura
        public DateTime validoEm 
        {
            get 
            {
                return DateTime.UtcNow;
            }
        }

        //Palavra base dentro das classes que herdam permite acessar membros da classe pai
    }
}
