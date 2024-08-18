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

        // Adicionar classe baseada na posi��o de rolagem
        if (scrollPercent >= 100) {
            $logo.addClass('green');
        } else if (scrollPercent >= 50) {
            $logo.addClass('golden');
        } else if (scrollPercent >= 10) {
            $logo.addClass('turquoise');
        }
    });

    // Fun��o para deslizar suavemente para a se��o
    $('a[href*="#"]').on('click', function (e) {
        /*e.preventDefault();*/

        var target = $(this.hash);
        if (target.length) {
            $('html, body').animate({
                scrollTop: target.offset().top
            }, 500, 'linear'); // Ajuste a dura��o conforme necess�rio
        }
    });


    //Funcao para tooltip
    $('.NavBar-link-wrapper.js-tooltip').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var toolTip = $(this).attr('aria-describedby');
        var buttonOffset = $(this).offset(); // Obt�m a posi��o do bot�o
        var buttonHeight = $(this).outerHeight(); // Obt�m a altura do bot�o

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

        // Clique fora do tooltip para escond�-lo
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

                $(document).off('click.hideTooltip'); // Remove o evento para evitar m�ltiplas liga��es
            }
        });
    });




    $('.NavBar-link-wrapper.js-tooltip-language').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var toolTip = $(this).attr('aria-describedby');
        var buttonOffset = $(this).offset(); // Obt�m a posi��o do bot�o
        var buttonHeight = $(this).outerHeight(); // Obt�m a altura do bot�o

        setTimeout(() => {
            $('#' + toolTip).attr('aria-hidden', 'false');

            $('#' + toolTip).css({
                'z-index': '9999',
                'visibility': 'visible',
                'transition-duration': '350ms',
                'position': 'absolute',
                'top': (buttonOffset.top - 65) + 'px', // 5px abaixo do bot�o
                //'left': buttonOffset.left + 'px',
                'will-change': 'transform'
            });

            $('#childToolTip2').css({
                'transition-duration': '375ms',
                'display': 'block'
            });


        }, 300);

        // Clique fora do tooltip para escond�-lo
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

                $(document).off('click.hideTooltip'); // Remove o evento para evitar m�ltiplas liga��es
            }
        });
    });


    

    // Impede que o clique dentro do tooltip feche ele
    $(document).on('click', '[id^="tippy-tooltip-"]', function (e) {
        e.stopPropagation();
    });


});

$(document).ready(function () {


    var previousDigits = [];

    // Fun��o para processar o n�mero e gerar os elementos
    function processNumber(number) {
        // Converte o n�mero em string usando a formata��o correta para inteiros
        var numberString = number.toLocaleString('en-US', { useGrouping: true, minimumFractionDigits: 0, maximumFractionDigits: 0 });
        var digits = numberString.replace(/,/g, '').split(''); // Remove as v�rgulas e divide em d�gitos
        var commaPositions = [];

        // Identifica as posi��es das v�rgulas
        var digitCounter = 0;
        for (var i = 0; i < numberString.length; i++) {
            if (numberString[i] === ',') {
                commaPositions.push(digitCounter);
            } else {
                digitCounter++;
            }
        }

        var $container = $('#number-container');

        // Se for a primeira execu��o, cria todos os elementos
        if (previousDigits.length === 0) {
            $container.empty();

            $.each(digits, function (i, digit) {
                var cardHtml = `
                    <div class="Tick">
                        <div class="upperCard"><span>${digit}</span></div>
                        <div class="lowerCard"><span>${digit+1}</span></div>
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
            // Atualiza apenas os d�gitos que mudaram usando append
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

                    $container.append(cardHtml); // Adiciona um novo elemento com o d�gito atualizado

                    if (commaPositions.includes(i + 1)) {
                        $container.append('<span class="comma">,</span>');
                    }
                }
            });

            // Remover os d�gitos excedentes
            if (digits.length < previousDigits.length) {
                $container.children('.Tick').slice(digits.length).remove();
            }

            // Gerencia a adi��o e remo��o das v�rgulas
            var commas = $container.find('.comma');
            commas.remove(); // Remove todas as v�rgulas antes de recalcular

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
                //bountyAmount = calcBounty(data);
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
                return undefined; // Retorna undefined se o objeto 'e' n�o estiver definido
            }

            // Obt�m o timestamp do objeto 'e' e converte para milissegundos
            var timestamp = new Date(e.timestamp).getTime();
            
            var currentTime = Date.now();

            // Calcula a diferen�a de tempo desde o timestamp at� agora
            var timeElapsed = currentTime - timestamp;
            
            // Calcula o valor restante da recompensa
            var remainingBounty = e.amount - Math.floor(1000000 / e.tick_speed);
            console.log(remainingBounty);
            return remainingBounty;
        }

        function bountyLoop() {
            var self = this; // Usa 'self' para preservar o contexto 'this'
            // Calcula um tempo aleat�rio para o pr�ximo loop
            var randomDelay = Math.floor(Math.random() * bounty.tick_speed * 4) + (bounty.tick_speed / 2);
            // Configura um timeout para o pr�ximo loop
            setTimeout(function () {

                // Decrementa o valor da recompensa
                bounty.amount -= 1;
                // Se o valor da recompensa ainda � maior que 50.000, chama o loop novamente
                if (bounty.amount > 1) {
                    processNumber(bounty.amount);
                    bountyLoop();
                }
            }, randomDelay);
        }

        
    }

    // Chama a fun��o getBounty para iniciar o processo
    getBounty();
});


$(document).ready(function () {


    $('.slick-track').slick({
        dots: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
    });

    // Esconde o conte�do more-content no carregamento da p�gina
    $('.more-content').hide();

    // Remove a classe active do bot�o more no carregamento da p�gina
    $('.more').removeClass('active');

    // Adiciona o evento de clique
    $('.more').on('click', function () {
        // Toggle da classe active no bot�o more
        $(this).toggleClass('active');

        // Toggle do conte�do correspondente ao bot�o clicado
        $(this).next('.more-content').slideToggle();
    });
});

function setCulture(element) {
    var culture = element.getAttribute('data-culture');

    fetch('/SetCulture', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value // Token de verifica��o antifalsifica��o
        },
        body: JSON.stringify({ culture: culture })
    })
        .then(response => {
            if (response.ok) {
                window.location.reload(); // Recarrega a p�gina para aplicar a nova cultura
            } else {
                console.error('Erro ao definir a cultura');
            }
        })
        .catch(error => console.error('Erro na solicita��o:', error));
}