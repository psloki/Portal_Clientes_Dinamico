"use strict";

function obtenerTresLetrasInicialesDeUltimosSeisMeses() {
    const meses = [
        "Enero",
        "Febrero",
        "Marzo",
        "Abril",
        "Mayo",
        "Junio",
        "Julio",
        "Agosto",
        "Septiembre",
        "Octubre",
        "Noviembre",
        "Diciembre",
    ];

    const hoy = new Date();
    let resultados = [];

    for (let i = 0; i < 6; i++) {
        const mesAnterior = new Date(hoy.getFullYear(), hoy.getMonth() - i, 1);
        const nombreMes = meses[mesAnterior.getMonth()];
        const tresLetras = nombreMes.slice(0, 3);
        resultados.unshift(tresLetras);
    }

    return resultados;
}
var initChartsWidget1 = function () {
    var element = document.getElementById("kt_charts_widget_1_chart");

    if (!element) {
        return;
    }

    var chart = {
        self: null,
        rendered: false
    };

    var initChart = function () {
        var height = parseInt(KTUtil.css(element, 'height'));
        var labelColor = KTUtil.getCssVariableValue('--bs-gray-500');
        var borderColor = KTUtil.getCssVariableValue('--bs-gray-200');
        var baseColor = KTUtil.getCssVariableValue('--bs-primary');
        var secondaryColor = KTUtil.getCssVariableValue('--bs-gray-300');

        var options = {
            series: [{
                name: 'Con atraso',
                data: [44, 55, 57, 56, 61, 58]
            }, {
                name: 'Sin atraso',
                data: [76, 85, 101, 98, 87, 105]
            }],
            chart: {
                fontFamily: 'inherit',
                type: 'bar',
                height: height,
                toolbar: {
                    show: false
                }
            },
            plotOptions: {
                bar: {
                    horizontal: false,
                    columnWidth: ['30%'],
                    borderRadius: [6]
                },
            },
            legend: {
                show: false
            },
            dataLabels: {
                enabled: false
            },
            stroke: {
                show: true,
                width: 2,
                colors: ['transparent']
            },
            xaxis: {
                categories: obtenerTresLetrasInicialesDeUltimosSeisMeses(),
                axisBorder: {
                    show: false,
                },
                axisTicks: {
                    show: false
                },
                labels: {
                    style: {
                        colors: labelColor,
                        fontSize: '12px'
                    }
                }
            },
            yaxis: {
                labels: {
                    style: {
                        colors: labelColor,
                        fontSize: '12px'
                    }
                }
            },
            fill: {
                opacity: 1
            },
            states: {
                normal: {
                    filter: {
                        type: 'none',
                        value: 0
                    }
                },
                hover: {
                    filter: {
                        type: 'none',
                        value: 0
                    }
                },
                active: {
                    allowMultipleDataPointsSelection: false,
                    filter: {
                        type: 'none',
                        value: 0
                    }
                }
            },
            tooltip: {
                style: {
                    fontSize: '12px'
                },
                y: {
                    formatter: function (val) {
                        return val
                    }
                }
            },
            colors: [baseColor, secondaryColor],
            grid: {
                borderColor: borderColor,
                strokeDashArray: 4,
                yaxis: {
                    lines: {
                        show: true
                    }
                }
            }
        };

        chart.self = new ApexCharts(element, options);
        chart.self.render();
        chart.rendered = true;
    }

    // Init chart
    initChart();

    // Update chart on theme mode change
    KTThemeMode.on("kt.thememode.change", function () {
        if (chart.rendered) {
            chart.self.destroy();
        }

        initChart();
    });
}

$(document).ready(function () {
    initChartsWidget1()
});


// On document ready
//jQuery(document).ready(function () {


//});