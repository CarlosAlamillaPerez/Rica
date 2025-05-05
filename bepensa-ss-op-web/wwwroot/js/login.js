function togglePassword() {
    var passwordField = $('#txtPassword');
    var currentType = passwordField.attr('type');

    if (currentType === 'password') {
        passwordField.attr('type', 'text');
        $('#eyeIcon').removeClass('fa-eye-slash');
    } else {
        passwordField.attr('type', 'password');
        $('#eyeIcon').addClass('fa-eye-slash');
    }
}

function toggleConfirmPassword() {
    var passwordField = $('#txtConfirmPassword');
    var currentType = passwordField.attr('type');

    if (currentType === 'password') {
        passwordField.attr('type', 'text');
        $('#eyeConfirmIcon').removeClass('fa-eye-slash');
    } else {
        passwordField.attr('type', 'password');
        $('#eyeConfirmIcon').addClass('fa-eye-slash');
    }
}