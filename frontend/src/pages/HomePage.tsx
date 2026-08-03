import { Alert, CircularProgress, Grid, Stack, Typography, Card, Button } from "@mui/material";
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

    return (
        <Stack spacing={2} sx={{ padding: 2, marginTop: 8 }}>
            <Typography variant="h1">Random Pairing</Typography>
            {isLoading && <CircularProgress />}
            {isError && <Alert severity="error">Error loading pairing.</Alert>}
            {pairing && (
                <Stack>
                    <Grid container spacing={2}>
                        {[pairing.firstMemberName, pairing.secondMemberName].map((name) => (
                            <Grid size={6} key={name}>
                                <Card sx={{ padding: 2, textAlign: 'center' }}>
                                    <Typography variant="h3">{name}</Typography>
                                </Card>
                            </Grid>
                        ))}
                    </Grid>
                    <Grid container spacing={2} sx={{ marginTop: 2 }}>
                        {[0, 1, 2, 3, 4, 5].map((rating) => (
                            <Grid size={2} key={rating}>
                                <Button variant="contained" color="primary" fullWidth onClick={() => handleRatePairing(rating)}>{rating}</Button>
                            </Grid>
                        ))}
                    </Grid>
                </Stack>
            )}
        </Stack>
    )
}