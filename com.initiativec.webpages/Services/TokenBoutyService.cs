using com.database;
using com.database.entities;
using Microsoft.AspNetCore.Mvc;

namespace com.initiativec.webpages.Services
{
    public class TokenBoutyService
    {
        private readonly DatabaseContext _context;

        public TokenBoutyService(DatabaseContext context)
        {
            _context = context;
        }

        public decimal OnGetBounty()
        {
            var tokenPool = _context.TokenPool.FirstOrDefault();

            decimal total = tokenPool.total;
            decimal divisor = tokenPool.divisor;

            var amount = CalculoProximaVaga(total, divisor);

            return amount;
        }

        private decimal CalculoProximaVaga(decimal total, decimal divisor)
        {
            return total / divisor;
        }

        public bool ReservarValorInicial(int usuarioId)
        {
            //var user = _context.Users.SingleOrDefault(u => u.id == usuarioId);
            var pool = _context.TokenPool.FirstOrDefault(t => t.id == 1);

            decimal total = pool.total;
            decimal divisor = pool.divisor;

            var amount = CalculoProximaVaga(total, divisor);

            TokenBounty bounty = new TokenBounty();
            bounty.id_usuario = usuarioId;
            bounty.valor_reserva_total = amount;
            bounty.valor_reservado = ReservaValorPercentual(amount, 10);

            pool.total = pool.total - bounty.valor_reservado;

            _context.TokenBounties.Add(bounty);
            _context.TokenPool.Update(pool);

            _context.SaveChanges();


            return true;
        }

        private decimal ReservaValorPercentual(decimal reservaTotal, int percentual)
        {
            decimal valorPercentual = (reservaTotal * percentual) / 100;
            valorPercentual = Math.Round(valorPercentual, 0, MidpointRounding.AwayFromZero);

            return valorPercentual;
        }

        public decimal ValorReservaPorConvite(int usuarioId)
        {
            var bounty = _context.TokenBounties.FirstOrDefault(b => b.id_usuario == usuarioId);
            decimal valorTodosConvites = 0;
            if (bounty != null)
            {
                valorTodosConvites = ReservaValorPercentual(bounty.valor_reserva_total, 40);
            }
            
            var valorCadaVerificacao = valorTodosConvites / 5;

            return valorCadaVerificacao;
        }

        public decimal ValorReservaConviteTotal(int usuarioId)
        {
            var bounty = _context.TokenBounties.FirstOrDefault(b => b.id_usuario == usuarioId);
            decimal valorTodosConvites = 0;
            
            if (bounty != null)
            {
                valorTodosConvites = ReservaValorPercentual(bounty.valor_reserva_total, 40);
            }
            

            return valorTodosConvites;
        }
    }
}
