document.addEventListener('DOMContentLoaded', function () {
    var svgContainer = document.getElementById('svgContainer');

    if (svgContainer) {
        var animItem = bodymovin.loadAnimation({
            wrapper: svgContainer,
            animType: 'svg',
            loop: true,
            path: 'https://gist.githubusercontent.com/kiriss91/889556a50a1e60560003453f8933d60b/raw/7bacaec5eac9301618bf89df4fc5510c8ae65e36/Terminal-payment.json'
        });
    } else {
        console.error('Element with id "svgContainer" not found.');
    }
});

