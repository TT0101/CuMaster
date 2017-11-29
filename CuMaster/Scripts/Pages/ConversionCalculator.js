$(document).ready(function () {
    bindAccordianClicksConverter();

    $("#fromCurrency, #toCurrency").change(function ()
    {
        OnCurrencyChange();
    });

    $("#currencyFromValue").change(function ()
    {
        OnFromValueChange();
    });

    $("#currencyToValue").change(function ()
    {
        OnToValueChange();
    });

    //$("#btnSaveConversion").click(function ()
    //{
    //    openSaveModal();
    //});

    $("#btnSaveEntry").click(function ()
    {
        saveConversion();
    });

    $("#btnClearConversion").click(function ()
    {
        var data = PopulateResponseObject();
        ajaxCallPost("ConversionCalculator", "ResetToDefault", data, loadNewValues);
    });
});

function bindAccordianClicksConverter()
{
    $("#converterCollapse").on('click', function ()
    {
        toggleCollapsePane("converterCollapse", "converterCollapsePane", "converterCollapseToggle");
    });
}

function OnCurrencyChange()
{
    var data = PopulateResponseObject();
    ajaxCallPostt("ConversionCalculator", "CurrencyChanged", data, loadNewValues);
    //$.ajax(
    //    {
    //        dataType: "json",
    //        data: data,
    //        url: $(location).attr('pathname') + "/" + callFunction,
    //        success: onSuccess,
    //        failure: onError
    //    });
}

function OnFromValueChange()
{
    var data = PopulateResponseObject();
    ajaxCallPost("ConversionCalculator", "FromValueChanged", data, loadNewValues);
}

function OnToValueChange()
{
    var data = PopulateResponseObject();
    ajaxCallPost("ConversionCalculator", "ToValueChanged", data, loadNewValues);
}

function PopulateResponseObject()
{
    var vals = new Object();
    vals.CurrencyFrom = new Object();
    vals.CurrencyTo = new Object();
    vals.CurrencyFrom.ID = $("#fromCurrency option:selected").val();
    vals.CurrencyTo.ID = $("#toCurrency option:selected").val();
    vals.ValueFrom = $("#currencyFromValue").val();
    vals.ValueTo = $("#currencyToValue").val();

    return vals;
}

function loadNewValues(responseData)
{
    $("#currencyFromValue").val(responseData.ValueFrom);
    $("#currencyToValue").val(responseData.ValueTo);
    $("#currencyFromDisplayName").text(responseData.CurrencyFrom.Name);
    $("#currencyToRate").text(responseData.FromRate);
    $("#currencyToDisplayName").text(responseData.CurrencyTo.Name);
    $("#dateLastUpdated").text(responseData.CurrencyFromLastUpdatedString);
    $("#currencyFromSymbol").html(responseData.CurrencyFrom.HTMLSymbol);
    $("#currencyToSymbol").html(responseData.CurrencyTo.HTMLSymbol);
}

function saveConversion()
{
    var data = CreateTrackerEntryObject();
    ajaxCallPost("ConversionTracker", "SaveNewTrackerEntry", data, onEntrySave);
}

function CreateTrackerEntryObject()
{
    var vals = new Object();
    vals.EntryName = $("#txtEntryName").val();
    vals.CurrencyFrom = $("#fromCurrency option:selected").val();
    vals.CurrencyTo = $("#toCurrency option:selected").val();
    vals.AmountFrom = $("#currencyFromValue").val();
    vals.AmountTo = $("#currencyToValue").val();
    vals.LastUpdatedString = $("#dateLastUpdated").text();
    vals.RateUsed = $("#hFromRate").val();

    return vals;
}
function onEntrySave(response)
{
    $("#btnCloseEntry").click();
    showChangeSuccessMessage(response);
}

//function openSaveModel()
//{

//}

//function LoadInitialConversion(returnData)
//{

//}

