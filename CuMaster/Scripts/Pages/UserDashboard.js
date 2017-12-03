$(document).ready(function ()
{
    bindAccordianClicksConverter();

    $("#defaultFromCurrency").change(function ()
    {
        OnCurrencyChange();
    });

   
    $("#btnSaveProfile").click(function ()
    {
        saveProfile();
    });

    $("#btnSaveDefaults").click(function ()
    {
        saveDefaults();
    });

    $("#txtEmail").change(function ()
    {
        checkEmail($(this).val());
    });
    
});

function bindAccordianClicksConverter()
{
    $("#profileCollapse").on('click', function ()
    {
        toggleCollapsePane("profileCollapse", "profileCollapsePane", "profileCollapseToggle");
    });

    $("#defaultCollapse").on('click', function ()
    {
        toggleCollapsePane("defaultCollapse", "defaultCollapsePane", "defaultCollapseToggle");
    });
}

function OnCurrencyChange()
{
    var data = PopulateResponseObject();
    ajaxCallPost("UserDashboard", "OnCurrencyChanged", data, loadNewValues);
}

function PopulateResponseObject()
{
    var vals = new Object();
    vals.UserName = $("#lblUserName").text();
    vals.Email = $("#txtEmail").val();
    vals.DisplayName = $("#txtDisplayName").val();
    vals.DefaultCurrencyFrom = $("#defaultFromCurrency option:selected").val();
    vals.DefaultCurrencyTo = $("#defaultToCurrency option:selected").val();
    vals.DefaultCountry = $("#defaultCountry option:selected").val();
    vals.AutoUpdateTrackerDefault = $("#chkAutoUpdate").is(":checked");

    vals.DefaultCurrencyFrom = (vals.DefaultCurrencyFrom == "") ? null : vals.DefaultCurrencyFrom;
    vals.DefaultCurrencyTo = (vals.DefaultCurrencyTo == "") ? null : vals.DefaultCurrencyTo;
    vals.DefaultCountry = (vals.DefaultCountry == "") ? null : vals.DefaultCountry;

    return vals;
}

function loadNewValues(responseData)
{
    loadDropdown("defaultToCurrency", responseData.CurrenciesTo);
    $("#defaultToCurrency option[value=" + responseData.DefaultCurrencyTo + "]").prop("selected", "selected");
}

function saveProfile()
{
    var data = PopulateResponseObject();
    ajaxCallPost("UserDashboard", "SaveProfile", data, onSave)
}

function saveDefaults()
{
    var data = PopulateResponseObject();
    ajaxCallPost("UserDashboard", "SaveDefaults", data, onSave)
}

function onSave(response)
{
    if (response.StatusKey == "ERROR")
    {
        showFailedMessage("dashboardFailed", function () { });
    }
    else
    {
        showSuccessMessage("dashboardSuccess", function () { });
    }
}

function checkEmail(email)
{
    var data = new Object();
    data.Email = email;
    ajaxCallPost("UserDashboard", "CheckEmail", data, onEmailCheckEnd);
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