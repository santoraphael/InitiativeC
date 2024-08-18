$(document).ready(function () {


    var previousDigits = [];

    // Função para processar o número e gerar os elementos
    function processNumber(number) {
        // Converte o número em string usando a formatação correta para inteiros
        var numberString = number.toLocaleString('en-US', { useGrouping: true, minimumFractionDigits: 0, maximumFractionDigits: 0 });
        var digits = numberString.replace(/,/g, '').split(''); // Remove as vírgulas e divide em dígitos
        var commaPositions = [];

        // Identifica as posições das vírgulas
        var digitCounter = 0;
        for (var i = 0; i < numberString.length; i++) {
            if (numberString[i] === ',') {
                commaPositions.push(digitCounter);
            } else {
                digitCounter++;
            }
        }

        var $container = $('#number-container');

        // Se for a primeira execução, cria todos os elementos
        if (previousDigits.length === 0) {
            $container.empty();

            $.each(digits, function (i, digit) {
                var cardHtml = `
                    <div class="Tick">
                        <div class="upperCard"><span>${digit}</span></div>
                        <div class="lowerCard"><span>${digit + 1}</span></div>
                        <div class="flipCard first fold"><span>${digit + 1}</span></div>
                        <div class="flipCard second unfold"><span>${digit}</span></div>
                    </div>
                `;

                $container.append(cardHtml);

                if (commaPositions.includes(i + 1)) {
                    $container.append('<span class="comma">,</span>');
                }
            });

        } else {
            // Atualiza apenas os dígitos que mudaram usando append
            $container.children('.Tick').each(function (i) {
                if (digits[i] !== previousDigits[i]) {
                    $(this).remove(); // Remove o elemento atual

                    var incrementedDigit = parseInt(digits[i]) + 1;

                    var cardHtml = `
                        <div class="Tick">
                            <div class="upperCard"><span>${digits[i]}</span></div>
                            <div class="lowerCard"><span>${incrementedDigit}</span></div>
                            <div class="flipCard first fold"><span>${incrementedDigit}</span></div>
                            <div class="flipCard second unfold"><span>${digits[i]}</span></div>
                        </div>
                    `;

                    $container.append(cardHtml); // Adiciona um novo elemento com o dígito atualizado

                    if (commaPositions.includes(i + 1)) {
                        $container.append('<span class="comma">,</span>');
                    }
                }
            });

            // Remover os dígitos excedentes
            if (digits.length < previousDigits.length) {
                $container.children('.Tick').slice(digits.length).remove();
            }

            // Gerencia a adição e remoção das vírgulas
            var commas = $container.find('.comma');
            commas.remove(); // Remove todas as vírgulas antes de recalcular

            $.each(commaPositions, function (i, pos) {
                $container.children('.Tick').eq(pos - 1).after('<span class="comma">,</span>');
            });
        }

        // Atualiza o estado anterior com o atual
        previousDigits = digits;
    }




    function getBounty() {
        var isError = false;
        var bounty = {};
        var bountyAmount = 0;


        $.get("/static/bounty")
            .done(function (data) {
                isError = false;
                bounty = data;
                processNumber(data.amount);
                bountyLoop();
                console.log("Recompensa obtida com sucesso:", bounty);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.error("Erro ao obter a recompensa:", textStatus, errorThrown);
                isError = true;
                bounty = {
                    timestamp: new Date(),
                    amount: 30000,
                    tick_speed: 200
                };
                bountyAmount = 30000;
                bountyLoop();
            });


        function calcBounty(e) {
            if (!e) {
                return undefined; // Retorna undefined se o objeto 'e' não estiver definido
            }

            // Obtém o timestamp do objeto 'e' e converte para milissegundos
            var timestamp = new Date(e.timestamp).getTime();

            var currentTime = Date.now();

            // Calcula a diferença de tempo desde o timestamp até agora
            var timeElapsed = currentTime - timestamp;

            // Calcula o valor restante da recompensa
            var remainingBounty = e.amount - Math.floor(1000000 / e.tick_speed);
            console.log(remainingBounty);
            return remainingBounty;
        }

        function bountyLoop() {
            var self = this; // Usa 'self' para preservar o contexto 'this'
            // Calcula um tempo aleatório para o próximo loop
            var randomDelay = Math.floor(Math.random() * bounty.tick_speed * 4) + (bounty.tick_speed / 2);
            // Configura um timeout para o próximo loop
            setTimeout(function () {

                // Decrementa o valor da recompensa
                bounty.amount -= 1;
                // Se o valor da recompensa ainda é maior que 50.000, chama o loop novamente
                if (bounty.amount > 1) {
                    processNumber(bounty.amount);
                    bountyLoop();
                }
            }, randomDelay);
        }


    }

    // Chama a função getBounty para iniciar o processo
    getBounty();
});