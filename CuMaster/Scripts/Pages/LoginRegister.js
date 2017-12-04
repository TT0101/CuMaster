function launchLoginRegister()
{
    $('#modelLoginRegister').modal('toggle');
}

$(document).ready(function ()
{
    $("#login-tab").click();

    $('[data-toggle="tooltip"]').tooltip({ 'placement': 'right' });

    initValiation("frmLogon");
    initValiation("frmRegister");

    $("#btnLogin").click(function ()
    {
        loginUser();
    });

    $("#btnRegister").click(function ()
    {
        registerUser();
    });

    $("#txtRegisterUserName").change(function ()
    {
        checkUserName($(this).val());
        $("#txtDisplayName").val($(this).val());
    });

    $("#txtRegisterEmail").change(function ()
    {
        checkEmail($(this).val());
    });

    $("#txtPasswordNew").change(function ()
    {
        checkPassword($(this).val());
    });

    $("#txtPasswordCheck").change(function ()
    {
        if ($(this).val() == $("#txtPasswordNew").val())
        {
            changeStatusSymbol("passwordCheckIsGood", true)
        }
        else
        {
            changeStatusSymbol("passwordCheckIsGood", false, "Passwords must match");
        }
    });

    $("#txtPassword", "#txtUserName").keypress(function (e)
    {
        if (e.which == 13)
        {
            loginUser();
        }
    });
});

function passwordsMatch()
{
    if ($("#txtPasswordCheck").val() == $("#txtPasswordNew").val())
    {
        return true;
    }
    return false;
}

function loginUser()
{
    var data = new Object();
    data.UserName = $("#txtUserName").val();
    data.Password = $("#txtPassword").val();

    ajaxCallPost("Login", "LoginUser", data, onLogin);
}

function onLogin(response)
{
    if (response.StatusKey == "ERROR")
    {
        showFailedMessage("loginFailed", function () { });
    }
    else
    {
        showSuccessMessage("loginSuccess", function () { $("#btnCloseLoginRegister").click(); });
        //setupMenu();
        window.location.reload();
    }
}


function registerUser()
{
    if ($("#frmRegister").valid() && passwordsMatch())
    {
        var data = new Object();
        data.UserName = $("#txtRegisterUserName").val();
        data.Email = $("#txtRegisterEmail").val();
        data.DisplayName = $("#txtDisplayName").val();
        data.Password = $("#txtPasswordCheck").val();
        ajaxCallPost("Register", "RegisterUser", data, onRegister);
    }
}

function onRegister(response)
{
    if (response.StatusKey == "ERROR")
    {
        showFailedMessage("registerFailed", function () { });
    }
    else
    {
        showSuccessMessage("registerSuccess", function () { });

        $("#login-tab").click();
        clearAllItems();
    }
}

function checkUserName(userName)
{
    var data = new Object();
    data.UserName = userName;
    ajaxCallPost("Register", "CheckUserName", data, onUserNameCheckEnd);
}

function onUserNameCheckEnd(response)
{
    if (response.Valid)
    {
        //change symbol
        changeStatusSymbol("userNameIsGood", true);
        $("#txtRegisterUserName").valid(true);
    }
    else
    {
        changeStatusSymbol("userNameIsGood", false, "User name is taken");
        $("#txtRegisterUserName").valid(false);
    }
}

function checkEmail(email)
{
    var data = new Object();
    data.Email = email;
    ajaxCallPost("Register", "CheckEmail", data, onEmailCheckEnd);
}

function onEmailCheckEnd(response)
{
    if (response.Valid)
    {
        //change symbol
        changeStatusSymbol("emailIsGood", true);
    }
    else
    {
        changeStatusSymbol("emailIsGood", false, "Email is already associated with an account");
    }
}

function checkPassword(pw)
{
    var data = new Object();
    data.Password = pw;
    data.UserName = $("#txtRegisterUserName").val();
    ajaxCallPost("Register", "CheckPassword", data, onPasswordCheckEnd);
}

function onPasswordCheckEnd(response)
{
    if (response.Valid)
    {
        //change symbol
        changeStatusSymbol("passwordNewIsGood", true);
    }
    else
    {
        changeStatusSymbol("passwordNewIsGood", false, "Passwords must have at least 8 characters, with numbers, special characters, an uppper and lower case letters");
    }
}

$("#modalLoginRegister").on('hidden.bs.modal', function ()
{
    clearAllItems();
});

function clearAllItems()
{
    $("#txtRegisterUserName").val('');
    $("#txtRegisterEmail").val('');
    $("#txtDisplayName").val('');
    $("#txtPasswordCheck").val('');
    $("#txtUserName").val('');
    $('#txtPassword').val('');
    clearValidation($("#frmLogin"));
    clearValidation($("#frmRegister"));
    changeStatusSymbol("emailIsGood", false)
    changeStatusSymbol("userNameIsGood", false)
    changeStatusSymbol("passwordNewIsGood", false)
    changeStatusSymbol("passwordCheckIsGood", false)
}

function changeStatusSymbol(symbolToChange, result, hoverText)
{
    if (result)
    {
        $("#" + symbolToChange).html('<span class="fa fa-check"></span>');
        $("#" + symbolToChange).removeClass('addon-danger').addClass('addon-success');
    }
    else
    {
        $("#" + symbolToChange).html('<span class="fa fa-ban" data-toggle="tooltip" data-placement="right" title="' + hoverText + '"></span>');
        $("#" + symbolToChange).addClass('addon-danger').removeClass('addon-success');
    }
}

function logOffUser()
{
    ajaxCallPost("Logon", "LogoffUser", "{}", onLogOff);
}

function onLogOff(response)
{
    setupMenu();
    alert("You have been successfully logged off.");
}
