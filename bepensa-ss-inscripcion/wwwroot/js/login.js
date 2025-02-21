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

$(document).ready(function () {
    $('#btnToggle').on('click', function () {
        togglePassword();
    });
});