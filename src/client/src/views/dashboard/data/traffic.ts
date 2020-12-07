import { colors, Theme } from '@material-ui/core';
import LaptopMacIcon from '@material-ui/icons/LaptopMac';
import PhoneIcon from '@material-ui/icons/Phone';
import TabletIcon from '@material-ui/icons/Tablet';

export const trafficData = {
    datasets: [
        {
            data: [63, 15, 22],
            backgroundColor: [
                colors.indigo[500],
                colors.red[600],
                colors.orange[600]
            ],
            borderWidth: 8,
            borderColor: colors.common.white,
            hoverBorderColor: colors.common.white
        }
    ],
    labels: ['Desktop', 'Tablet', 'Mobile']
};

export const getTrafficOptions = (theme: Theme) => ({
    animation: false,
    cutoutPercentage: 80,
    layout: { padding: 0 },
    legend: {
        display: false
    },
    maintainAspectRatio: false,
    responsive: true,
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

export const devices = [
    {
        title: 'Desktop',
        value: 63,
        icon: LaptopMacIcon,
        color: colors.indigo[500]
    },
    {
        title: 'Tablet',
        value: 15,
        icon: TabletIcon,
        color: colors.red[600]
    },
    {
        title: 'Mobile',
        value: 23,
        icon: PhoneIcon,
        color: colors.orange[600]
    }
];
