import { Alert, CircularProgress, Grid, Stack, Typography, Card, Button, LinearProgress } from "@mui/material";
import { useGetRandomPairing, useRatePairingMutation } from "../features/pairings/hooks/usePairing";

export default function HomePage() {
    // Start by loading a random pairing
    const { data: pairing, isLoading, isError, refetch } = useGetRandomPairing();
    const mutation = useRatePairingMutation();

    const handleRatePairing = (rating: number) => {
        if (pairing) {
            mutation.mutate({ pairingId: pairing.pairingId, rating });
            refetch();
        }
    }

    const ratingEmoji = (rating: number) => {
        switch (rating) {
            case 0: return "🥶"
            case 1: return "😖"
            case 2: return "🤨"
            case 3: return "😏"
            case 4: return "😍"
            case 5: return "🥵"
            default: return "❓"
        }
    }

    return (
        <Stack spacing={2} sx={{ padding: 2, marginTop: 12 }}>
            <Typography variant="h1">Couple Smash??</Typography>
            <Typography variant="body1">Disregarding sexuality, how good of a couple do you think the following two gritty people would make?</Typography>
            <Alert severity="info">Note: real life couples have been excluded from this experiment</Alert>
            {/* It's very important to inundate the user with progress bars */}
            {isLoading && <Stack spacing={1}>
                <LinearProgress />
                <LinearProgress variant="query" color="secondary" />
                <LinearProgress color="success" />
                <CircularProgress size="4rem" sx={{ marginTop: 2 }} />
                </Stack>}
            {isError && <Alert severity="error">Error loading pairing.</Alert>}
            {pairing && (
                <Stack spacing={2}>
                    <Grid container spacing={2}>
                        <Grid size={5}>
                            <Card sx={{ padding: 2, textAlign: 'center' }}>
                                <Typography variant="h3">{pairing.firstMemberName}</Typography>
                            </Card>
                        </Grid>
                        <Grid size={2}>
                            <Card sx={{ padding: 2, textAlign: 'center' }}>
                                <Typography variant="h3">&</Typography>
                            </Card>
                        </Grid>
                        <Grid size={5}>
                            <Card sx={{ padding: 2, textAlign: 'center' }}>
                                <Typography variant="h3">{pairing.secondMemberName}</Typography>
                            </Card>
                        </Grid>
                    </Grid>
                    <Typography variant="h3" sx={{ textAlign: 'center' }}>Compatibility</Typography>
                    <Grid container spacing={2} sx={{ marginTop: 2 }}>
                        {[0, 1, 2, 3, 4, 5].map((rating) => (
                            <Grid size={2} key={rating}>
                                <Button variant="text" color="primary" 
                                    onClick={() => handleRatePairing(rating)} 
                                    sx={{ borderRadius: '50%', width: '100%', aspectRatio: '1 / 1'}}>
                                    <Stack>
                                    <Typography variant="h2">{ratingEmoji(rating)}</Typography>
                                    <Typography variant="h5">{rating}</Typography>
                                    </Stack>
                                </Button>
                            </Grid>
                        ))}
                    </Grid>
                </Stack>
            )}
        </Stack>
    )
}