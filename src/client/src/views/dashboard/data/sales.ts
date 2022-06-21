import { colors, Theme } from '@mui/material';

export const salesData = {
    datasets: [
        {
            barThickness: 12,
            maxBarThickness: 10,
            barPercentage: 0.5,
            categoryPercentage: 0.5,
            backgroundColor: colors.indigo[500],
            data: [18, 5, 19, 27, 29, 19, 20],
            label: 'This year'
        },
        {
            barThickness: 12,
            maxBarThickness: 10,
            barPercentage: 0.5,
            categoryPercentage: 0.5,
            backgroundColor: colors.grey[200],
            data: [11, 20, 12, 29, 30, 25, 13],
            label: 'Last year'
        }
    ],
    labels: ['1 Aug', '2 Aug', '3 Aug', '4 Aug', '5 Aug', '6 Aug']
};

export const getSalesOptions = (theme: Theme) => ({
    cornerRadius: 20,
    layout: { padding: 0 },
    legend: { display: false },
    maintainAspectRatio: false,
    responsive: true,
    scales: {
        xAxes: {
            ticks: {
                color: theme.palette.text.secondary
            },
            gridLines: {
                display: false,
                drawBorder: false
            }
        },
        yAxes: {
            ticks: {
                color: theme.palette.text.secondary,
                beginAtZero: true,
                min: 0
            },
            gridLines: {
                borderDash: [2],
                borderDashOffset: [2],
                color: theme.palette.divider,
                drawBorder: false,
                zeroLineBorderDash: [2],
                zeroLineBorderDashOffset: [2],
                zeroLineColor: theme.palette.divider
            }
        }
    },
    tooltips: {
        backgroundColor: theme.palette.background.default,
        bodyFontColor: theme.palette.text.secondary,
        borderColor: theme.palette.divider,
        borderWidth: 1,
        enabled: true,
        footerFontColor: theme.palette.text.secondary,
        intersect: false,
        mode: 'index',
        titleFontColor: theme.palette.text.primary
    }
});