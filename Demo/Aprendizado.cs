using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

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

        //StartDate (data de início) e EndDate (data de término)//
    }
}