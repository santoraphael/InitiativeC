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


        setTimeout(() => {

            $('#' + toolTip).attr('aria-hidden', 'false');

            $('#' + toolTip).css({
                'z-index': '9999',
                'visibility': 'visible',
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




    $('.NavBar-link-wrapper').on('click', function (e) {
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
    function fetchBountyData() {
        $.ajax({
            url: '/static/bounty',
            method: 'GET',
            success: function (data) {
                // Atualizar os tickers com os dados recebidos
                updateTickers(data);
            },
            error: function (error) {
                console.error('Erro ao buscar dados de bounty:', error);
            }
        });
    }

    function updateTickers(data) {
        // Supondo que os dados sejam uma lista de números
        let values = data.values;
        console.log(values);
        $('.Tick').each(function (index) {
            let value = values[index] || 0;
            let upperCard = $(this).find('.upperCard span');
            let lowerCard = $(this).find('.lowerCard span');
            let flipCardFirst = $(this).find('.flipCard.first span');
            let flipCardSecond = $(this).find('.flipCard.second span');

            // Atualizar valores das cartas
            upperCard.text(value);
            lowerCard.text(value - 1);
            flipCardFirst.text(value - 1);
            flipCardSecond.text(value);

            // Adicionar classes de animação
            $(this).find('.flipCard').addClass('fold unfold');

            // Remover classes de animação após a animação terminar
            setTimeout(() => {
                $(this).find('.flipCard').removeClass('fold unfold');
            }, 500);
        });
    }

    // Buscar dados a cada X segundos
    setInterval(fetchBountyData, 5000);
    // Chamada inicial
    fetchBountyData();
});
