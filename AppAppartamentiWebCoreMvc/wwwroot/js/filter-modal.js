$(document).ready(function () {
    var sliderPrezzo = document.getElementById('sliderPrezzo');
    noUiSlider.create(sliderPrezzo, {
        start: [50000, 150000],
        tooltips: [wNumb({
            mark: '.',
            thousand: ',',
            prefix: '€ ',
            suffix: '',
            decimals: 0
        }), wNumb({
            mark: '.',
            thousand: ',',
            prefix: '€ ',
            suffix: '',
            decimals: 0
        })],
        connect: true,
        step: 10000,
        range: {
            'min': 30000,
            'max': 500000
        }
    });

    var sliderDimensione = document.getElementById('sliderDimensione');
    noUiSlider.create(sliderDimensione, {
        start: [80, 150],
        tooltips: [wNumb({ decimals: 0, suffix: ' m²' }), wNumb({ decimals: 0, suffix: ' m²', })
        ],
        connect: true,
        step: 50,
        range: {
            'min': 50,
            'max': 500
        }
    });

    //This button will increment the value
    $('[data-quantity="plus"]').click(function (e) {
        //Stop acting like a button
        e.preventDefault();

        //Get the field name
        fieldName = $(this).attr('data-field');
        //Get its current value

        var currentVal = parseInt($('input[name=' + fieldName + ']').val());
        // If is not undefined

        if (!isNaN(currentVal)) {
            //Increment
            $('input[name=' + fieldName + ']').val(currentVal + 1);
        } else {
            //Otherwise put a 0 there

            $('input[name=' + fieldName + ']').val(0);
        }
    });

    //This button will decrement the value till
    $('[data-quantity="minus"]').click(function (e) {

        //Stop acting like a button
        e.preventDefault();

        //Get the field name
        fieldName = $(this).attr('data-field');

        //Get its current value
        var currentVal = parseInt($('input[name=' + fieldName + ']').val());

        //If it isn't undefined or its greater than 0
        if (!isNaN(currentVal) && currentVal > 0) {

            //Decrement one
            $('input[name=' + fieldName + ']').val(currentVal - 1);
        } else {

            //Otherwise put a 0 there
            $('input[name=' + fieldName + ']').val(0);
        }
    });
});

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    document.cookie = name + '=; Max-Age=-99999999;';
}


