import { Alert, CircularProgress, Grid, Stack, Typography, Card, Button } from "@mui/material";
import { useGetRandomPairing } from "../features/pairings/hooks/usePairing";

export default function HomePage() {
    // Start by loading a random pairing
    const { data: pairing, isLoading, isError } = useGetRandomPairing();

    return (
        <Stack spacing={2} sx={{ padding: 2, marginTop: 8 }}>
            <Typography variant="h1">Random Pairing</Typography>
            {isLoading && <CircularProgress />}
            {isError && <Alert severity="error">Error loading pairing.</Alert>}
            {pairing && (
                <Stack>
                    <Grid container spacing={2}>
                        <Grid size={6}>
                            <Card sx={{ padding: 2, textAlign: 'center' }}>
                                <Typography variant="h3">{pairing.firstMemberName}</Typography>
                            </Card>
                        </Grid>
                        <Grid size={6}>
                            <Card sx={{ padding: 2, textAlign: 'center' }}>
                                <Typography variant="h3">{pairing.secondMemberName}</Typography>
                            </Card>
                        </Grid>
                    </Grid>
                    <Grid container spacing={2} sx={{ marginTop: 2 }}>
                        <Grid size={2}>
                            <Button variant="contained" color="primary" fullWidth>0</Button>
                        </Grid>
                        <Grid size={2}>
                            <Button variant="contained" color="primary" fullWidth>1</Button>
                        </Grid>
                        <Grid size={2}>
                            <Button variant="contained" color="primary" fullWidth>2</Button>
                        </Grid>
                        <Grid size={2}>
                            <Button variant="contained" color="primary" fullWidth>3</Button>
                        </Grid>
                        <Grid size={2}>
                            <Button variant="contained" color="primary" fullWidth>4</Button>
                        </Grid>
                        <Grid size={2}>
                            <Button variant="contained" color="primary" fullWidth>5</Button>
                        </Grid>
                    </Grid>
                </Stack>
            )}
        </Stack>
    )
}