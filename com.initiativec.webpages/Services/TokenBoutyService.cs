using com.database;
using com.database.entities;
using com.initiativec.webpages.ViewModel;
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

        /// <summary>
        /// Reserva um valor específico para uma tarefa executada por um usuário.
        /// Garante que a reserva não exceda o valor total do bounty do usuário e que o pool tenha saldo suficiente.
        /// </summary>
        /// <param name="usuarioId">ID do usuário que está reservando o valor.</param>
        /// <param name="valorReserva">Valor a ser reservado para a tarefa.</param>
        /// <returns>Retorna true se a reserva foi bem-sucedida; caso contrário, false.</returns>
        public bool ReservarValorTarefa(int usuarioId, decimal valorReserva)
        {
            // Validar entradas
            if (usuarioId <= 0)
            {
                // ID de usuário inválido
                return false;
            }

            if (valorReserva <= 0)
            {
                // Valor de reserva inválido
                return false;
            }

            // Iniciar uma transação para garantir a consistência dos dados
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Obter o pool de tokens
                    var pool = _context.TokenPool.FirstOrDefault(t => t.id == 1);
                    if (pool == null)
                    {
                        // Pool não encontrado
                        return false;
                    }

                    // 2. Verificar se há saldo suficiente no pool
                    if (pool.total < valorReserva)
                    {
                        // Saldo insuficiente no pool
                        return false;
                    }

                    // 3. Obter o bounty do usuário
                    var bounty = _context.TokenBounties.FirstOrDefault(b => b.id_usuario == usuarioId);
                    if (bounty == null)
                    {
                        // Bounty do usuário não encontrado, criar um novo registro
                        bounty = new TokenBounty
                        {
                            id_usuario = usuarioId,
                            valor_reserva_total = 0, // Assumindo que 0 é o valor inicial; ajuste conforme necessário
                            valor_reservado = 0
                        };
                        _context.TokenBounties.Add(bounty);
                    }

                    // 4. Verificar se a nova reserva excede o valor total do bounty
                    decimal reservaDisponivel = bounty.valor_reserva_total - bounty.valor_reservado;
                    if (valorReserva > reservaDisponivel)
                    {
                        // Reserva excede o valor disponível no bounty do usuário
                        return false;
                    }

                    // 5. Atualizar as reservas no bounty
                    bounty.valor_reservado += valorReserva;

                    // 6. Deduzir o valor do pool
                    pool.total -= valorReserva;

                    // 7. Atualizar as entidades no contexto
                    _context.TokenPool.Update(pool);
                    _context.TokenBounties.Update(bounty);

                    // 8. Salvar as mudanças no banco de dados
                    _context.SaveChanges();

                    // 9. Confirmar a transação
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    // Em caso de erro, reverter a transação
                    transaction.Rollback();
                    // Opcional: Registrar o erro para diagnóstico
                    // Logger.LogError(ex, "Erro ao reservar valor para tarefa.");
                    return false;
                }
            }
        }


        public CardSaldoAtual ObterReservaPercentuais(int usuarioId, int percentualTask, CardSaldoAtual cardSaldoAtual)
        {
            // Validação básica dos parâmetros
            if (usuarioId <= 0)
            {
                throw new ArgumentException("ID de usuário inválido.", nameof(usuarioId));
            }

            if (percentualTask <= 0 || percentualTask > 100)
            {
                throw new ArgumentException("Percentual da tarefa deve ser maior que 0 e menor ou igual a 100.", nameof(percentualTask));
            }

            // Obter o bounty do usuário
            var bounty = _context.TokenBounties.FirstOrDefault(b => b.id_usuario == usuarioId);


            // Calcular o percentual já reservado
            int percentualReservado = (int)((bounty.valor_reservado / bounty.valor_reserva_total) * 100);
            //percentualReservado = Math.Round(percentualReservado, 2);

            // Garantir que o percentual total não exceda 100%
            if (percentualTask + percentualReservado > 100)
            {
                // Ajustar o percentualTask para não exceder o limite
                percentualTask = 100 - percentualReservado;
            }

            // Garantir que o percentualTask não seja negativo
            if (percentualTask < 0)
            {
                percentualTask = 0;
            }

            // Arredondar os valores para duas casas decimais
            //percentualTask = Math.Round(percentualTask, 2);


            cardSaldoAtual.percentualAtual = percentualTask + percentualReservado;
            cardSaldoAtual.percentualReservado = percentualReservado;

            return cardSaldoAtual;
        }
    }
}
