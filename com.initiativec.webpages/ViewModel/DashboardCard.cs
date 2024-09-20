using com.database.entities;

namespace com.initiativec.webpages.ViewModel
{
    public class CardVerificaConvite
    {
        public int ConvitesParaRevisar { get; set; }
        public int Aprovados { get; set; }
        public int ConvitesDisponiveis { get; set; }
        public List<UsuarioAcao> Usuarios { get; set; }
        public bool status { get; set; }
    }

    public class UsuarioAcao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class  CardEnviarConvite
    {
        public decimal valorConviteTotal { get; set; }
        public string invite_code { get; set; }
        public bool status { get; set; }
    }

    public class CardSaldoAtual
    {
        public decimal valorSaldo { get; set; }
        public decimal valorSaldoTotal { get; set; }
        public int percentualAtual { get; set; }
        public int percentualReservado  { get; set; }
        public bool status { get; set; }
    }

    public class FollowStatusCard
    {
        public string SourceUsername { get; set; }
        public string TargetUsername { get; set; }
        public bool IsFollowing { get; set; }
        public string ErrorMessage { get; set; }
    }
}
