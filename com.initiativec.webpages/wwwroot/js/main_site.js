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
        /*e.preventDefault();*/

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
                'top': (buttonOffset.top + 35) + 'px', // 5px abaixo do botão
                'left': (buttonOffset.left - 75) + 'px',
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


    $('.mobile-btn').click(function () {
        const menu = document.querySelector('.MobileMenu');
        menu.classList.toggle('active');
    });

    $('.MobileMenu').click(function () {

        const clickedElement = event.target.closest('.MobileMenu-link.mobile-js-tooltip-language');

        if (clickedElement) {
            // Se clicou no botão de linguagens, exibe ou esconde a tooltip
            const tooltip = document.querySelector('.tippy-popper');
            if (tooltip) {
                // Alterna a visibilidade da tooltip
                const isVisible = tooltip.style.visibility === 'visible';
                tooltip.style.visibility = isVisible ? 'hidden' : 'visible';
            }
        } else {
            // Se não clicou no botão de linguagens, faz o resto
            const menu = document.querySelector('.MobileMenu');
            menu.classList.toggle('active');
        }
    });

    $('.LanguagesMenu-link').click(function () {
        const menu = document.querySelector('.MobileMenu');
        menu.classList.toggle('active');
    });


});




$(document).ready(function () {


    $('.slick-track').slick({
        dots: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
    });

    // Esconde o conteúdo more-content no carregamento da página
    $('.more-content').hide();

    // Remove a classe active do botão more no carregamento da página
    $('.more').removeClass('active');

    // Adiciona o evento de clique
    $('.more').on('click', function () {
        // Toggle da classe active no botão more
        $(this).toggleClass('active');

        // Toggle do conteúdo correspondente ao botão clicado
        $(this).next('.more-content').slideToggle();
    });

   
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