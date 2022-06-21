import React from 'react';
import Button from '@mui/material/Button';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import CardHeader from '@mui/material/CardHeader';
import Grid from '@mui/material/Grid';
import StarIcon from '@mui/icons-material/StarBorder';
import Typography from '@mui/material/Typography';
import { makeStyles } from '@mui/styles';
import { Tier } from './content';

interface Props {
    tiers: Tier[];
}

const useStyles = makeStyles((theme) => ({
    cardHeader: {
        backgroundColor:
            theme.palette.mode === 'light' ? theme.palette.grey[200] : theme.palette.grey[700],
    },
    cardPricing: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'baseline',
        marginBottom: theme.spacing(2),
    },
}));


const Tiers = ({ tiers }: Props) => {
    const classes = useStyles();

    return (<>
        {tiers.map((tier) => (
            // Enterprise card is full width at sm breakpoint
            <Grid item key={tier.title} xs={12} sm={tier.title === 'Enterprise' ? 12 : 6} md={4}>
                <Card>
                    <CardHeader
                        title={tier.title}
                        subheader={tier.subheader}
                        titleTypographyProps={{ align: 'center' }}
                        subheaderTypographyProps={{ align: 'center' }}
                        action={tier.title === 'Pro' ? <StarIcon /> : null}
                        className={classes.cardHeader}
                    />
                    <CardContent>
                        <div className={classes.cardPricing}>
                            <Typography component="h2" variant="h3" color="textPrimary">
                                ${tier.price}
                            </Typography>
                            <Typography variant="h6" color="textSecondary">
                                /mo
                            </Typography>
                        </div>
                        <ul>
                            {tier.description.map((line) => (
                                <Typography component="li" variant="subtitle1" align="center" key={line}>
                                    {line}
                                </Typography>
                            ))}
                        </ul>
                    </CardContent>
                    <CardActions>
                        <Button fullWidth variant={tier.buttonVariant} color="primary">
                            {tier.buttonText}
                        </Button>
                    </CardActions>
                </Card>
            </Grid>
        ))}
    </>);
};

export default Tiers;