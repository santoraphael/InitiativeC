namespace com.initiativec.webpages.ViewModel
{
    public class DashboardVM
    {
        public int id_usuario { get; set; }
        public string nome_usuario { get; set; }
        public int qtd_dias_restantes { get; set; }
        public int qtd_convites_restantes { get; set; }
        public decimal amount_token_por_convite { get; set; }
    }
}
