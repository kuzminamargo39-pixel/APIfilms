namespace APIfilms.Models.DTO
{
    public class UtilisateurDetailDto
    {
        public int UserId { get; set; }
        public string Nom { get; set; } = null!;        
        public string Mail { get; set; } = null!;

        public string RoleDescription { get; set; } = "Standard User";
    }
}