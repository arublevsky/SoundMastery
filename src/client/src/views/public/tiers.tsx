import React from 'react';
import Button from '@material-ui/core/Button';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardHeader from '@material-ui/core/CardHeader';
import Grid from '@material-ui/core/Grid';
import StarIcon from '@material-ui/icons/StarBorder';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import { Tier } from './content';

interface Props {
    tiers: Tier[];
}

const useStyles = makeStyles((theme) => ({
    cardHeader: {
        backgroundColor:
            theme.palette.type === 'light' ? theme.palette.grey[200] : theme.palette.grey[700],
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