

var x = document.cookie;
if (x === 'ok') {
    console.log("fEIL PASSORD");
    var head = document.getElementsByTagName('HEAD')[0];

    // Create new link Element 
    var link = document.createElement('link');

    // set the attributes for link element  
    link.rel = 'stylesheet';

    link.type = 'text/css';

    link.href = '/Styles/shake.css';

    alert("Password or Username did not match");

    // Append link element to HTML head 
    head.appendChild(link);



} else {
    console.log("oke");
    document.cookie = "ok";

    console.log(x);
}


// Real-time username checks

$('#username').keyup(function () {
    Console.log('#username');
    checkSpaces($(this));
    checkRemoveLengthError($(this));
    checkAvailability($(this));
    checkRemovePresenceError($(this));
});

// Real-time password checks
$('#password').keyup(function () {
    checkRemoveLengthError($(this));
    checkRemovePresenceError($(this));
});

// Check lengths on blur
$('#username, #password').blur(function () {
    checkLength($(this));
});

// Check all on submit
$('#submit').click(function (event) {
    event.preventDefault();

    var valid_username = checkSpaces($('#username')) &&
        checkLength($('#username')) &&
        checkPresence($('#username')) &&
        checkAvailability($('#username'));
    var valid_password = checkLength($('#password')) && checkPresence($('#password'));

    if (valid_username && valid_password) {
        alert('Success!');
    }
});

// Error checking functions

function checkSpaces(target) {
    var value = target.val();
    var label = target.prev('.info').children('.spaces');

    if (value.includes(' ')) {
        label.addClass('error');
    } else {
        label.removeClass('error');
        return true;
    }
}

function checkLength(target) {
    var value = target.val();
    var label = target.prev('.info').children('.length');

    if (value.length > 0 && value.length < 5) {
        label.addClass('error');
    } else {
        label.removeClass('error');
        return true;
    }
}

function checkRemoveLengthError(target) {
    var value = target.val();
    var label = target.prev('.info').children('.length');

    if (value.length == 0 || value.length >= 5) {
        label.removeClass('error');
    }
}

function checkAvailability(target) {
    // Pseudo AJAX check of whether name is available
    var value = target.val();
    var label = target.prev('.info').children('.availability');

    if (value == 'stelios') {
        label.removeClass('hidden');
    } else {
        label.addClass('hidden');
        return true;
    }
}

function checkPresence(target) {
    var value = target.val();
    var label = target.siblings('label');

    if (value.length == 0) {
        label.addClass('error');
    } else {
        label.removeClass('error');
        return true;
    }
}

function checkRemovePresenceError(target) {
    var value = target.val();
    var label = target.siblings('label');

    if (value.length > 0) {
        label.removeClass('error');
    }
}