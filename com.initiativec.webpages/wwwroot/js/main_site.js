// A $( document ).ready() block.
$(document).ready(function () {
    

    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        
        if (scroll > 50) {
            
            $('.NavBar.sticky').addClass('active');
        } else {
            $('.NavBar.sticky').removeClass('active');
        }

        setTimeout(() => {
            $('#tippy-tooltip-2').attr('aria-hidden', 'true');

            $('#tippy-tooltip-2').css({
                'z-index': '9999',
                'visibility': 'hidden',
                'transition-duration': '350ms',
                'position': 'absolute',
                'will-change': 'transform'
            });

            $('#childToolTip2').css({
                'transition-duration': '375ms',
                'display': 'none'
            });
        }, 300);


        //Alterando o cor do logo
        var scrollTop = $(this).scrollTop();
        var documentHeight = $(document).height();
        var windowHeight = $(window).height();

        var scrollPercent = (scrollTop / (documentHeight - windowHeight)) * 100;

        var $logo = $('.Logo');

        // Remover todas as classes de estado do logo
        $logo.removeClass('green golden turquoise ');

        // Adicionar classe baseada na posição de rolagem
        if (scrollPercent >= 100) {
            $logo.addClass('green');
        } else if (scrollPercent >= 50) {
            $logo.addClass('golden');
        } else if (scrollPercent >= 10) {
            $logo.addClass('turquoise');
        }
    });

    // Função para deslizar suavemente para a seção
    $('a[href*="#"]').on('click', function (e) {
        e.preventDefault();

        var target = $(this.hash);
        if (target.length) {
            $('html, body').animate({
                scrollTop: target.offset().top
            }, 500, 'linear'); // Ajuste a duração conforme necessário
        }
    });


    //Funcao para tooltip
    $('.NavBar-link-wrapper.js-tooltip').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var toolTip = $(this).attr('aria-describedby');
        var buttonOffset = $(this).offset(); // Obtém a posição do botão
        var buttonHeight = $(this).outerHeight(); // Obtém a altura do botão

        var toolTip = $(this).attr('aria-describedby');


        setTimeout(() => {

            $('#' + toolTip).attr('aria-hidden', 'false');

            $('#' + toolTip).css({
                'z-index': '9999',
                'visibility': 'visible',
                'transition-duration': '350ms',
                'position': 'absolute',
                'transform': 'translate3d(333px, 63px, 0px)',
                'top': (buttonOffset.top) + 'px',
                'left': '0px',
                'will-change': 'transform'
            });


            $('#childToolTip3').css({
                'transition-duration': '375ms',
                'bottom': '0px'
            }).addClass('enter tippy-notransition').removeClass('leave');

        }, 300);

        // Clique fora do tooltip para escondê-lo
        $(document).on('click.hideTooltip', function (event) {
            if (!$(event.target).closest('#' + toolTip).length) {

                setTimeout(() => {

                    $('#' + toolTip).attr('aria-hidden', 'false');

                    $('#' + toolTip).css({
                        'z-index': '9999',
                        'visibility': 'hidden',
                        'transition-duration': '350ms',
                        'position': 'absolute',
                        'transform': 'translate3d(333px, 63px, 0px)',
                        'top': '0px',
                        'left': '0px',
                        'will-change': 'transform'
                    });

                    $('#childToolTip3').css({
                        'transition-duration': '375ms',
                        'bottom': '0px'
                    }).removeClass('enter tippy-notransition').addClass('leave');

                }, 300);

                $(document).off('click.hideTooltip'); // Remove o evento para evitar múltiplas ligações
            }
        });
    });




    $('.NavBar-link-wrapper.js-tooltip-language').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var toolTip = $(this).attr('aria-describedby');
        var buttonOffset = $(this).offset(); // Obtém a posição do botão
        var buttonHeight = $(this).outerHeight(); // Obtém a altura do botão

        setTimeout(() => {
            $('#' + toolTip).attr('aria-hidden', 'false');

            $('#' + toolTip).css({
                'z-index': '9999',
                'visibility': 'visible',
                'transition-duration': '350ms',
                'position': 'absolute',
                'top': (buttonOffset.top - 65) + 'px', // 5px abaixo do botão
                //'left': buttonOffset.left + 'px',
                'will-change': 'transform'
            });

            $('#childToolTip2').css({
                'transition-duration': '375ms',
                'display': 'block'
            });


        }, 300);

        // Clique fora do tooltip para escondê-lo
        $(document).on('click.hideTooltip', function (event) {
            if (!$(event.target).closest('#' + toolTip).length) {
                setTimeout(() => {
                    $('#' + toolTip).attr('aria-hidden', 'true');

                    $('#' + toolTip).css({
                        'z-index': '9999',
                        'visibility': 'hidden',
                        'transition-duration': '350ms',
                        'position': 'absolute',
                        'will-change': 'transform'
                    });


                    $('#childToolTip2').css({
                        'transition-duration': '375ms',
                        'display': 'none'
                    });
                }, 300);

                $(document).off('click.hideTooltip'); // Remove o evento para evitar múltiplas ligações
            }
        });
    });


    

    // Impede que o clique dentro do tooltip feche ele
    $(document).on('click', '[id^="tippy-tooltip-"]', function (e) {
        e.stopPropagation();
    });


});

$(document).ready(function () {
    function getBounty() {
        var isError = false;
        var bounty = {};
        var bountyAmount = 0;

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
            var remainingBounty = e.amount - Math.floor(timeElapsed / e.tick_speed);

            return remainingBounty;
        }

        function bountyLoop() {
            var self = this; // Usa 'self' para preservar o contexto 'this'

            // Calcula um tempo aleatório para o próximo loop
            var randomDelay = Math.floor(Math.random() * self.bounty.tick_speed * 4) + (self.bounty.tick_speed / 2);

            // Configura um timeout para o próximo loop
            setTimeout(function () {
                // Decrementa o valor da recompensa
                self.bountyAmount -= 1;

                // Se o valor da recompensa ainda é maior que 50.000, chama o loop novamente
                if (self.bountyAmount > 50000) {
                    self.bountyLoop();
                }
            }, randomDelay);
        }

        $.get("/static/bounty")
            .done(function (data) {
                isError = false;
                bounty = data;
                bountyAmount = calcBounty(data);
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
    }

    // Chama a função getBounty para iniciar o processo
    //getBounty();
});


function setCulture(element) {
    var culture = element.getAttribute('data-culture');

    fetch('/SetCulture', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value // Token de verificação antifalsificação
        },
        body: JSON.stringify({ culture: culture })
    })
        .then(response => {
            if (response.ok) {
                window.location.reload(); // Recarrega a página para aplicar a nova cultura
            } else {
                console.error('Erro ao definir a cultura');
            }
        })
        .catch(error => console.error('Erro na solicitação:', error));
}
