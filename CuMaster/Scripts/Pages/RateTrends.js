var _selectedRates = new Array();
var _selectedRatesOld = new Array();
var _onCurrencyChange = false;

$(document).ready(function ()
{
    bindAccordianClicksRates();
    //_selectedRates.push("EUR");
    initChart(_selectedRates);
    _onCurrencyChange = true;
    initRateList();
    $("#ddBaseCurrency").change(function ()
    {
        OnCurrencyChange();
    });

    $("#ddTimeSpan").change(function ()
    {
        OnTimeChange();
    });

    $("#btnResetChart").click(function ()
    {
        resetChart(_selectedRates, _selectedRates[0]); //this needs to be in chart.js...just remove all but first one...
    });

});

function bindAccordianClicksRates()
{
    $("#filterCollapse").on('click', function ()
    {
        toggleCollapsePane("filterCollapse", "filterCollapsePane", "filterCollapseToggle");
    });
}

function OnCurrencyChange()
{
    _onCurrencyChange = true;
    var data = PopulateResponseObject();
    ajaxCallPost("RateList", "CurrencyChanged", data, loadRateList);
}

function OnTimeChange()
{
    //var data = PopulateResponseObject();

    //ajaxCallPost("RateChart", "TimeSpanChanged", data, loadRateChart);
    var data = new Object();
    data.CurrencyFrom = $("#ddBaseCurrency option:selected").val();
    data.Days = $("#ddTimeSpan option:selected").val();

    reloadRateChart(data);
}

//function GetLengthFromButton(btnName)
//{
//    return btnName.substr(3);
//}

function PopulateResponseObject()
{
    var vals = new Object();
    vals.BaseCurrency = $("#ddBaseCurrency option:selected").val();
    vals.LengthSelected = $("#ddTimeSpan option:selected").val();

    return vals;
}

function loadRateList(response)
{
    _onCurrencyChange = true;
    refreshRateDataTableForBaseChange(response.BaseCurrency); //in ratellist.js
}

///chart
var _chart;

function initChart(rData)
{
    _chart = c3.generate({
        bindto: '#rateChart',
        padding:
        {
            right: 50
        },
        color:
        {
            pattern: ['#1f77b4', '#aec7e8', '#ff7f0e', '#ffbb78', '#2ca02c', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
        },
        data:
        {
            x: 'x',
            xFormat: '%m/%d/%Y',
            type: 'line',
            rows: createBasicChartArray(rData.Data, rData.CurrencyTo), //takes dictionary of x,y coords
            empty: { label: { text: "No Data Available" } }
        },
        axis:
        {
            x:
            {
                type: 'timeseries',
                tick:
                {
                    format: '%m/%d/%y'
                }
            },
            y:
            {
                padding:
                {
                    bottom: 2
                },
                tick:
                {
                    format: function (x)
                    {
                        return (x == Math.floor(x)) ? x : "";
                    },
                    min: 0
                }
            }
        }
    });

    //_chart = c3.generate({
    //    bindto: '#rateChart',
    //    data: {
    //        columns: [
    //            ['data1', 300, 350, 300, 0, 0, 0],
    //            ['data2', 130, 100, 140, 200, 150, 50]
    //        ],
    //        types: {
    //            data1: 'area',
    //            data2: 'area-spline'
    //        }
    //    },
    //    axis: {
    //        y: {
    //            padding: {
    //                bottom: 0
    //            },
    //            min: 0
    //        },
    //        x: {
    //            padding: {
    //                left: 0
    //            },
    //            min: 0,
    //            show: false
    //        }
    //    }
    //});
   
}

function loadRateChart(rData)
{
    initChart(rData);
}

function addToRateChart(rData)
{
    _chart.load(
        {
            columns: createBasicChartArray(rData.Data, rData.CurrencyTo)
        });
}

function resetChart(rateList, rateToLoad)
{
    var data = new Object();
    data.CurrencyFrom = $("#ddBaseCurrency option:selected").val();
    data.Days = $("#ddTimeSpan option:selected").val();;
    data.CurrencyTo = rateToLoad;
    getHistoricalRateData(data);
    _chart.load(
    {
         unload: rateList,
    });
}

function removeFromRateChart(currencyTo)
{
    _chart.load(
        {
            unload: currencyTo,
        });
}

function reloadRateChart(data)
{
    $.each(_selectedRates, function (index)
    {
        data.CurrencyTo = _selectedRates[index];
        getHistoricalRateData(data);
    });
}

function getHistoricalRateData(data)
{
     ajaxCallPost("RateChart", "GetRateTrendData", data, addToRateChart);
}

///list

function initRateList()
{
    $("#rateTable").DataTable(
        {
            columns:
            [
                { data: null, name: "selectHint", width: "20px", title: " ", sortable: false },
                { data: null, name: 'CurrencyTo', title: " " },
                { data: null, name: 'Rate', title: " " }

            ],
            autoWidth: false,
            deferRender: true,
            processing: true,
            serverSide: true,
            order: [[1, 'asc']],
            createdRow: function (row, data, index)
            {
                if (_selectedRates.length == 0 && _onCurrencyChange)
                {
                    _selectedRates.push(data.CurrencyTo);
                    resetChart(_selectedRatesOld, data.CurrencyTo);
                    _onCurrencyChange = false;
                }

                $('td', row).eq(0).html("<span class='fa fa-line-chart'></span>");

                var dateUpdated = convertToLocalTime(data.DateUpdatedUTCStr);
                $('td', row).eq(1).html("<span class='font-weight-bold'>" + data.CurrencyToName + " (" + data.CurrencyTo + ")" + "</span><br/><small class='text-muted'>" + dateUpdated + "</small>");
                var rateMarkup = "<span>" + data.RateFowards.toFixed(roundCurrencyTo(data.IsToRateCrypto)) + "</span><br/><small class='text-muted'>Inverse: " + data.RateInverse.toFixed(roundCurrencyToInverse(data.IsFromRateCrypto)) + "</small>";
                $('td', row).eq(2).html(rateMarkup);
            },
            fnServerData: function (sSource, aoData, fnCallback)
            {
                var sendData = new Object();
                sendData.Params = createDataTableParams(aoData);
                sendData.BaseCurrency = $("#hBase").val();

                _selectedRatesOld = _selectedRates;
                _selectedRates = new Array();

                $.ajax
                    ({
                        dataType: "json",
                        type: "POST",
                        data: sendData,
                        url: siteRoot + "/RateList/GetRatesForBase",
                        success: function (msg)
                        {
                            if (msg != undefined)
                            {
                                fnCallback(msg);
                            }
                        },
                        error: function (obj, textStatus, errorThrown)
                        {
                            fnCallback({ data: [], recordsTotal: 0, recordsFiltered: 0 });
                            ajaxCallFailed(obj, textStatus, errorThrown);
                        }
                    });
            }
        }
    );

    $("#rateTable_length").addClass("text-left");
    initOnRowClick();

}

function refreshRateDataTableForBaseChange(base)
{
    $("#hBase").val(base);
    refeshRateDataTable();
}

function refeshRateDataTable()
{
    $("#rateTable").DataTable().ajax.reload();
}

function initOnRowClick()
{
    $('#rateTable tbody').on('click', 'tr', function ()
    {
        $(this).toggleClass('active');

        if ($(this).hasClass('active'))
        {
            _selectedRates.push(data.CurrencyTo);
            var data = new Object();
            data.CurrencyFrom = $("#ddBaseCurrency option:selected").val();
            data.Days = $("#ddTimeSpan option:selected").val();
            data.CurrencyTo = data.CurrencyTo;
            getHistoricalRateData(data);
        }
        else
        {
            _selectedRates.pop(data.CurrencyTo);
            //call remove currency from chart
            removeFromRateChart(data.CurrencyTo);
        }


    });
}

