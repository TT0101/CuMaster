_activeTimer = null;
_activeTimerTo = null;

$(document).ready(function ()
{
    bindAccordianClicksConverter();

    $("#fromCurrency, #toCurrency").change(function ()
    {
        OnCurrencyChange();
    });

    $("#currencyFromValue").keyup(function ()
    {
        if (_activeTimer == null)
        {
            _activeTimer = window.setTimeout(function () { OnFromValueChange(); _activeTimer = null }, 60);
        }
        
    });

    $("#currencyToValue").keyup(function ()
    {
        if (_activeTimerTo == null)
        {
            _activeTimerTo = window.setTimeout(function () { OnToValueChange(); _activeTimerTo = null }, 60);
        }
        
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

    $("#dateLastUpdated").text(convertToLocalTime($("#dateLastUpdated").text()));
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
    ajaxCallPost("ConversionCalculator", "CurrencyChanged", data, loadNewValues);
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
    //add functionality so that if timestamp is an hour old, get from server instead....
    var lastUpdated = moment($("#hLastUpdated").val());
    if (moment().diff(lastUpdated, 'hours') >= 1)
    {
        var data = PopulateResponseObject();
        ajaxCallPost("ConversionCalculator", "FromValueChanged", data, loadNewValues);
    }
    else
    {
        $("#currencyFromValue").val();
        $("#currencyToValue").val(calculateRate($("#currencyFromValue").val()));
    }
}

function OnToValueChange()
{
    //ditto above
    var lastUpdated = moment($("#hLastUpdated").val());
    if (moment().diff(lastUpdated, 'hours') >= 1)
    {
        var data = PopulateResponseObject();
        ajaxCallPost("ConversionCalculator", "ToValueChanged", data, loadNewValues);
    }
    else
    {
        $("#currencyFromValue").val(calculateReverseRate($("#currencyToValue").val()));
    }
}

function calculateRate(valueToConvert)
{
    var val = (valueToConvert * $("#hFromRate").val());
    var roundTo = roundCurrencyTo($("#hIsCrypto").val());
  
    return val.toFixed(roundTo);
}

function calculateReverseRate(valueToConvert)
{
    var rate = $("#hFromRate").val();
    var roundTo = roundCurrencyTo($("#hIsCrypto").val());
    if (rate == 0)
        return 0;
    else
    {
        var val = ((valueToConvert * (1 / rate)));
        
        return val.toFixed(roundTo);
    }
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
    loadDropdown("toCurrency", responseData.CurrenciesTo);
    $("#toCurrency option[value=" + responseData.CurrencyTo.ID + "]").prop("selected", "selected");

    $("#currencyFromValue").val(responseData.ValueFrom);
    $("#currencyToValue").val(responseData.ValueTo);
    $("#currencyFromDisplayName").text(responseData.CurrencyFrom.Name);
    $("#currencyToRate").text(responseData.FromRate);
    $("#currencyToDisplayName").text(responseData.CurrencyTo.Name);
    $("#dateLastUpdated").text(convertToLocalTime(responseData.CurrencyFromLastUpdatedUTCString));
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

