const radios = document.querySelectorAll('input[name="slider"]');

radios.forEach((radio, index) => {
    radio.addEventListener('change', () => {
        if (index === radios.length - 1) {
            setTimeout(() => {
                radios[0].checked = true;
            }, 3000); // tempo em milissegundos para voltar ao primeiro
        }
    });
});